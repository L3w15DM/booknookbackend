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



            // ratings sum
            double ratingsAvg = reviews.Select(r => r.Rating).Average();

            
            
            BookDetailsDto bookDetails = new BookDetailsDto
            {
                Reviews = reviews, 
                AverageRating = ratingsAvg
            };


            return StatusCode(200, bookDetails);
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{

        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

