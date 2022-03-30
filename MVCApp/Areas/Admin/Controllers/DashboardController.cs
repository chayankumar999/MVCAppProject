using ApiApp.AppDBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVCApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        private readonly ApplicationDbContext _context;

        public DashboardController(ILogger<AccountController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Index()
        {

            ViewBag.article = "Article";
            ViewBag.category = "Category";
            ViewBag.comment = "Comment";
            ViewBag.articleCount = _context.Articles.Count();
            ViewBag.categoryCount = _context.Categories.Count();
            ViewBag.commentCount  = _context.Comments.Count();
            return View();
        }
    }
}
