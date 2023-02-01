using Microsoft.AspNetCore.Mvc;

namespace Cities.API.Controllers
{
    // Attribute that "improves development experience" when building API
    // E.g. automatically returning 400 bad request on bad input
    [ApiController]
    // Creates a base path for all following HTTP actions
    // [controller] = matches the class name's prefix (i.e. api/cities) 
    [Route("api/[controller]")]
    // ControllerBase = contains helper methods in setting up controller
    public class CitiesController : ControllerBase
    {
        // URI ends with "api/cities" since that was placed in Route param
        [HttpGet]
        // Want to return everything in JSON format 
        // Create constructor that returns JSON-ified data
        public JsonResult GetCities() {
            return new JsonResult(CitiesDataStore.Current.Cities);
             
        }

        [HttpGet("id/{id}")]
        public JsonResult GetCityById(int id) {

            return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == id));

        }

        // This didn't work... 
        [HttpGet("cityname/{cityName}")]
        public JsonResult GetCityByName(string cityName) {
            return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Name == cityName));
        }


     
    }
}
