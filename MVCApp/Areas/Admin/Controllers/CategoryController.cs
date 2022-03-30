using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using ApiApp.AppDBContext;
using ApiApp.Models;

namespace MVCApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public CategoryController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> CategoryList()
        {
            return View(await _context.Categories.ToListAsync());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Id,Name,CreationTime")] Category category)
        {

            if (ModelState.IsValid)
            {
                var query = _context.Categories.FirstOrDefault(c => c.Name == category.Name);
                if(query != null)
                {
                    return RedirectToAction(nameof(CategoryList));
                }
                category.CreationTime = DateTime.Now.ToString();
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CategoryList));
            }
            return View(category);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CreationTime")] Category category, string edit, string delete)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(delete == "Delete")
                    {
                        _context.Remove(category);

                        var artilces = await _context.Articles.Where(a => a.Category == category.Name).ToListAsync();
                        foreach(var item in artilces)
                        {
                            item.Category = "Uncategorized";
                            _context.Update(item);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        category.CreationTime = DateTime.Now.ToString();
                        _context.Update(category);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CategoryList));
            }
            return View(category);
        }
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
