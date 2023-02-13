﻿using Cities.API.Models;
using Cities.API.Services;
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
        private readonly ICityInfoRepository _cityInfoRespository;

        // Dependency injected repository
        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
           
            _cityInfoRespository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));

        }
        // URI ends with "api/cities" since that was placed in Route param
        [HttpGet]
        // Want to return everything in JSON format 
        // Create constructor that returns JSON-ified data
        public ActionResult<IEnumerable<CityDto>> GetCities() {

            var cityEntities = _cityInfoRespository.GetCitiesAsync();


            // return Ok(_citiesDataStore.Cities);

            // No "Not Found" (empty collection = still a valid response) 
             
        }

        [HttpGet("id/{id}")]
        // ActionResult = imported from ControllerBase ("action" = Http get/put/post)
        public ActionResult<CityDto> GetCityById(int id) {

            var cityReturned = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == id);

            if (cityReturned == null)
            {
                return NotFound();
            }

            return Ok(cityReturned);

        }

   
        [HttpGet("cityname/{cityName}")]
        public ActionResult<CityDto> GetCityByName(string cityName) {

            var cityReturned = _citiesDataStore.Cities.FirstOrDefault(city => city.Name == cityName);

            if (cityReturned == null)
            {
                return NotFound();
            }

            // Otherwise, return the object with a 200 status code
            return Ok(cityReturned);

            // If error occurs that is not handled, automatically returns 500 internal server error 
        }


     
    }
}
