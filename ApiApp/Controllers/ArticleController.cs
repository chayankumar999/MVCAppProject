using ApiApp.AppDBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArticleController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("search")]
        public IActionResult Search(string? title, string? category)
        {
            try
            {
                var articleList = _context.Articles.Where(x => x.Publish == 1);
                var articles = from article in articleList
                               select new
                               {
                                   Title = article.Title,
                                   Category = article.Category,
                                   CreationTime = article.CreationTime
                               };

                if (!string.IsNullOrEmpty(title))
                {
                    articles = articles.Where(a => (a.Title.ToLower().Contains(title.ToLower())));
                }

                if (!string.IsNullOrEmpty(category))
                {
                    articles = articles.Where(a => a.Category.ToLower().Contains(category.ToLower()));
                }

                return Ok(articles);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("getarticlelist")]
        public IActionResult GetArticleList()
        {
            try
            {
                var articleList = _context.Articles.Where(x => x.Publish == 1);
                var articles = from article in articleList
                               select new
                               {
                                   Id = article.Id,
                                   Title = article.Title,
                                   Category = article.Category,
                                   CreationTime = article.CreationTime
                               };

                return Ok(articles);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("getarticlesbyid")]
        public IActionResult GetArticleById(int id)
        {
            try
            {
                var article = _context.Articles.FindAsync(id).Result;
                if(article == null || article.Publish == 0) return NotFound();
                return Ok(article);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }


        [HttpGet]
        [Route("getcategorylist")]

        public async Task<IActionResult> GetCategoryList()
        {
            try
            {
                var categoryList = await _context.Categories.ToListAsync();
                return Ok(categoryList);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }

        }
    }
       
}
