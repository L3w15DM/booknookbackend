using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.DataTransferObjects;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FullStackAuth_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        [HttpGet("{bookId}")]
        public IActionResult Get(string bookId)
        {
            
            List<ReviewWithUserDto> reviews = _context.Reviews.Where(b => b.BookId == bookId).Include(b => b.User).Select(r => new ReviewWithUserDto
                {
                    Id = r.Id,
                    BookId = r.BookId,
                    Text = r.Text,
                    Rating = r.Rating,
                    User = new UserForDisplayDto
                    {
                        Id = r.User.Id,
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName,
                        UserName = r.User.UserName
                    }
                }).ToList();

            // Favorite is false unless user is logged in
            bool favorite = false;
            // Find userId
            string userId = User.FindFirstValue("id");
            // If user is logged in show favorites
            if (userId != null)
            {
                favorite = _context.Favorites.Where(f => f.UserId == userId).Select(f => f.BookId).ToList().Contains(bookId);
            }


            // ratings sum
            double ratingsAvg = reviews.Select(r => r.Rating).Average();

            
            
            BookDetailsDto bookDetails = new BookDetailsDto
            {
                Reviews = reviews, 
                AverageRating = ratingsAvg,
                IsFavorite = favorite
            };


            return StatusCode(200, bookDetails);
        }

       
    }
}

