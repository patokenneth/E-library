using LibraryProject.Infrastructure.Interfaces;
using LibraryProject.ViewModel.RentViewModel;
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
    public class RentController : ControllerBase
    {
        private readonly IRent rentrepo;
        private readonly ILogger<RentController> logger;

        public RentController(IRent Rentrepo, ILogger<RentController> logger)
        {
            rentrepo = Rentrepo;
            this.logger = logger;
        }

        [HttpGet]
        [Route("BookDetailsById/{id}")]
        public async Task<IActionResult> GetBookDetailsByIdAsync(int id)
        {
            if (Request.Headers["Username"] == "user")
            {
                return new JsonResult(await rentrepo.GetBookDetailsAsync(id));
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route(template: "Checkout")]
        public IActionResult CheckOutBooks(CheckOutViewModel model)
        {
            if (Request.Headers["Username"] == "user" && Request.Headers["Admin"] == "1")
            {
                logger.LogInformation("A user has just accessed the CheckOutBooks endpoint of RentController");

                if (ModelState.IsValid)
                {
                    bool result = rentrepo.CheckOutBooks(model.ISBN, model);
                    if (result)
                    {
                        logger.LogInformation("Successfully checked out books");
                        return Ok(new { message = "successfully checked out book"});
                    }
                    else
                    {
                        return StatusCode(500);
                    }
                }
                else
                {
                    return BadRequest(new { message = "Kindly fill all the fields" });
                }
            }
            else
            {
                logger.LogError($"User not authorized to checkout books.");
                return StatusCode(403);
            }
        }

        [HttpPost]
        [Route(template:"Checkin")]
        public IActionResult CheckInBooks(CheckInViewModel model)
        {
            if (Request.Headers["Username"] == "user" && Request.Headers["Admin"] == "1")
            {
                if (ModelState.IsValid)
                {
                    logger.LogInformation($"CheckInBooks endpoint accessed");

                    if (model.ISBN.Length > 0 && !string.IsNullOrEmpty(model.Phone))
                    {
                        bool result = rentrepo.CheckInBooks(model.ISBN, model.Phone);

                        if (result)
                        {
                            logger.LogInformation("Successfully checked in books");
                            return Ok(new { message = "Book has been checked in successfully" });
                        }

                        else
                        {
                            return StatusCode(500);
                        }
                    }

                    return BadRequest();

                }

                else
                {
                    ModelState.AddModelError("", "Kindly fill all the required fields");
                }

            }

            return Unauthorized();
        }
    }
}
