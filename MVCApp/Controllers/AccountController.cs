using ApiApp.AppDBContext;
using ApiApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MVCWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;


        string BaseUrl = "https://localhost:44321/";

        public AccountController(ILogger<AccountController> logger, ApplicationDbContext context, IWebHostEnvironment hostEnvironment, 
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _logger = logger;
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (model == null)
            {
                return NotFound();
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.PostAsJsonAsync("/api/Authentication/login", model);
                if (Res.IsSuccessStatusCode)
                {

                    await signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);

                    string JWTtokenString = Res.Content.ReadAsStringAsync().Result;
                    JWT jwt = JsonConvert.DeserializeObject<JWT>(JWTtokenString);
                    HttpContext.Session.SetString("token", jwt.Token);
                    return RedirectToAction("index", "Home");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (model == null)
            {
                return View();
            }


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.PostAsJsonAsync("/api/Authentication/register", model);

                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction("login");
                }
            }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("login");
        }

        public async Task<IActionResult> UserProfile()
        {
            var username = User.Identity.Name;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                HttpResponseMessage Res = await client.GetAsync(string.Format("/api/Authentication/getuserdetails/?username={0}", username));
                if(Res.IsSuccessStatusCode)
                {
                    User _user = JsonConvert.DeserializeObject<User>(Res.Content.ReadAsStringAsync().Result);
                    return View(_user);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (model == null)
            {
                return View();
            }


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                HttpResponseMessage Res = await client.PostAsJsonAsync("/api/Authentication/changepassword", model);
                if (Res.IsSuccessStatusCode)
                {
                    await signInManager.SignOutAsync();
                    await signInManager.PasswordSignInAsync(model.Username, model.NewPassword, isPersistent: false, lockoutOnFailure: false);
                    return RedirectToAction("userProfile");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPassword model)
        {
            if (model == null)
            {
                return View();
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.PostAsJsonAsync("/api/Authentication/forgetpassword", model);
                if (Res.IsSuccessStatusCode)
                {
                    string password = JsonConvert.DeserializeObject<string>(Res.Content.ReadAsStringAsync().Result);
                    ViewBag.Password = password;
                    return View();
                }
            }

            return View();
        }



    }
}
