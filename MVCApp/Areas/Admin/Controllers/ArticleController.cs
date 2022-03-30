using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiApp.AppDBContext;
using ApiApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MVCApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ArticleController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> ArticleList()
        {
            return View(await _context.Articles.ToListAsync());
        }


        public async Task<IActionResult> Add()
        {
            IList<string> categoryList = _context.Categories.Select(c => c.Name).ToList();
            ViewBag.CategoryList = categoryList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Id,Title,Category,ArtBody,Publish,CreationTime")] Article article)
        {
            if (ModelState.IsValid)
            {
                article.CreationTime = DateTime.Now.ToString();
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ArticleList));
            }
            return View(article);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            IList<string> categoryList = _context.Categories.Select(c => c.Name).ToList();
            ViewBag.CategoryList = categoryList;

            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Category,ArtBody,Publish,CreationTime")] Article article, string edit, string delete)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(delete !=null && delete == "Delete")
                    {
                        var articleDelete = await _context.Articles.FindAsync(id);
                        _context.Articles.Remove(articleDelete);
                    }
                    else
                    {
                        article.CreationTime = DateTime.Now.ToString();
                        _context.Update(article);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ArticleList));
            }
            return View(article);
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
