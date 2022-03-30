using ApiApp.AppDBContext;
using ApiApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace MVCApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger, ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]

        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult login(AdminLogin model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.AdminUsers.FirstOrDefault(x => x.UserName == model.Username);
                if (user != null && user.Password == model.Password)
                {
                    _context.CurrentAdminUser.Add(user);
                    _context.SaveChangesAsync();
                    return RedirectToAction("index", "Dashboard");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }

        public IActionResult logout()
        {
            var user = _context.CurrentAdminUser.ToList();
            if (user.Any())
            {
                AdminUser cuser = _context.CurrentAdminUser.First();
                _context.CurrentAdminUser.Remove(cuser);
                _context.SaveChanges();
            }
            return RedirectToAction("login");
        }

        public IActionResult UserProfile()
        {
            AdminUser cuser = _context.CurrentAdminUser.First();
            if (cuser != null)
            {
                return View(cuser);
            }
            return NotFound();
        }
    }
}
