using LibraryProject.Infrastructure.Interfaces;
using LibraryProject.Model;
using LibraryProject.ViewModel.BookViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBook bookrepo;
        private readonly ILogger<BookController> logger;

        public BookController(IBook bookrepo, ILogger<BookController> logger)
        {
            this.bookrepo = bookrepo;
            this.logger = logger;
        }

        [HttpGet]
        [Route("ListAllBooks")]
        public async Task<IActionResult> GetBooksAsync()
        {
            if (Request.Headers["Username"] == "user")
            {
                return new JsonResult(await bookrepo.GetBooksAsync());
            }

            return Unauthorized(new { message = "Only validated users are allowed to access this resource" });
            
            
        }

        [HttpGet]
        [Route(template: "Search/{status}")]
        public async Task<IActionResult> SerachBookAsync(string searchvalue = "", bool status = false)
        {

            if (Request.Headers["Username"] == "user")
            {
                logger.LogInformation($"User searched for books with '{searchvalue}'");

                try
                {
                    return Ok(await bookrepo.SearchBookAsync(searchvalue, status));
                }
                catch (Exception ex)
                {
                    logger.LogInformation($"Error occurred while searching for '{searchvalue}' in BookController. {ex.Message}");

                    return NotFound(new { messge = $"No record found for '{searchvalue}'" });
                }
            }
            else
            {
                return Unauthorized();
            }


        }

        [HttpPost]
        [Route(template:"CreateBook")]
        public IActionResult CreateBook(CreateBookViewModel model)
        {
            //check if the headers contain the necessary key-value pairs

            if (Request.Headers["Username"] == "user" && Request.Headers["Admin"] == "1")
            {
                if (ModelState.IsValid)
                {

                    logger.LogInformation($"The CreateBook endpoint was accessed by admin");
                    bool result = bookrepo.CreateBook(model);
                    if (result)
                    {
                        logger.LogInformation($"New Book with the ISBN {model.ISBN} created by admin");

                        return Ok(new { message = "Successfully created book" });
                    }
                    else
                    {

                        return UnprocessableEntity(model);
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Kindly fill all the fields");
                    return BadRequest(new { message = "All the fields are required" });
                }
            }
            else
            {
                return Unauthorized();
            }
            

        }
    }
}
