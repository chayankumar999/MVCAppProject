using ApiApp.AppDBContext;
using ApiApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MVCApp.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public ArticleController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        string BaseUrl = "https://localhost:44321/";

        public async Task<IActionResult> ArticleList()
        {
            List<Article> articleList = new List<Article>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("/api/Article/getarticlelist");
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    articleList = JsonConvert.DeserializeObject<List<Article>>(Response);
                }


                ViewBag.signin = signInManager.IsSignedIn(User);
                return View(articleList);
            }
        }


        public async Task<IActionResult> ArticleDetails(int ? id)
        {
            if (id == null)
            {
                return View();
            }

            Article article = new Article();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync(string.Format("/api/Article/getarticlesbyid/?id={0}", id));
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    article = JsonConvert.DeserializeObject<Article>(Response);
                }
            }
            if (article == null)
            {
                return NotFound();
            }

            List<Comment> commentList = new List<Comment>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                HttpResponseMessage Res = await client.GetAsync(string.Format("/api/Comment/getcommentlistbyarticleid/?id={0}", id));
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    commentList = JsonConvert.DeserializeObject<List<Comment>>(Response);
                }
            }

            ViewModel viewModel = new ViewModel();
            viewModel.article = article;
            viewModel.CommentList = commentList;

            ViewBag.username = User.Identity.Name;
            ViewBag.artid = id;

            return View(viewModel);
        }

        public async Task<IActionResult> NewComment(Comment model)
        {
            if(model == null)
            {
                return NotFound(ModelState);
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                HttpResponseMessage Res = await client.PostAsJsonAsync("/api/Comment/addnewcomment", model);

                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction("ArticleDetails", new { id = model.Id});
                }
            }
            return RedirectToAction("ArticleDetails", new { id = model.Id });
        }

    }
}
