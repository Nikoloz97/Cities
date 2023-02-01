using Microsoft.AspNetCore.Mvc;

namespace Cities.API.Controllers
{
    // Attribute that "improves development experience" when building API
    // E.g. automatically returning 400 bad request on bad input
    [ApiController]
    // ControllerBase = contains helper methods in setting up controller
    public class CitiesController : ControllerBase
    {

        [HttpGet("api/cities")]
        // Want to return everything in JSON format 
        public JsonResult GetCities()
        {
            // create constructor that returns JSON-ified data
            return new JsonResult(
                new List<object>
                {
                    new { id = 1, Name = "New York City"},
                    new { id = 2, Name = "Antwerp"}
                });

        }
     
    }
}
