using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FullStackAuth_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Favorites
        [HttpGet, Authorize]
        public IActionResult Get()
        {
            List<Favorite> myFavorites = _context.Favorites.Where(f => f.UserId == User.FindFirstValue("id")).ToList();
            return StatusCode(200, myFavorites);
        }


        // POST api/values
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] Favorite book)
        {
            //Check current user to get id. 
            var userId = User.FindFirstValue("id");
            //Check if it is favorite already
            var alreadyFavorited = _context.Favorites.Where(f => f.BookId == book.BookId).Where(f => f.UserId == userId).ToList();
            if (!alreadyFavorited.Any())
            {
                book.UserId = userId;
                _context.Favorites.Add(book);
                _context.SaveChanges();
            }
            else
            {
                return Conflict(new { error = "Resource Already Exists", message = "You already favorited this book." });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return StatusCode(201, book);
        }

       

        
    }
}

