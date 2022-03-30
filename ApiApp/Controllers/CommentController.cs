using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using ApiApp.Models;
using ApiApp.AppDBContext;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _configuration;
        private ApplicationDbContext _context;

        public CommentController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ApplicationDbContext context)
        {
            this.userManager = userManager;
            _configuration = configuration;
            _context = context;
        }
      
        [HttpGet]
        [Route("getcommentlist")]
        public async Task<ActionResult> GetComments()
        {
            try
            {
                return Ok(await _context.Comments.ToListAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("getcommentlistbyarticleid")]
        public async Task<ActionResult> GetCommentsByArticleId(int id)
        {
            try
            {
                var commentList = await _context.Comments.Where(com => com.ArtId == id).ToListAsync(); 
                return Ok(commentList);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        [Route("addnewcomment")]
        public async Task<IActionResult> NewComment([FromBody] Comment model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null)
            {

                Comment newComment = new Comment()
                {
                    ArtId = model.ArtId,
                    Username = model.Username,
                    Comments = model.Comments,
                    CreationTime = DateTime.Now.ToString()
                };


                _context.Comments.Add(newComment);

                _context.SaveChanges();
             
                return Ok(new Response { Status = "Success", Message = "Comment created successfully!" });
            }
            return NotFound();
        }
    }
}
