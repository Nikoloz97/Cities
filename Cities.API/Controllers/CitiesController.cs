using AutoMapper;
using Cities.API.Models;
using Cities.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        // Dependency injected repository (i.e. 1. Created parameter, 2. Mapped parameter to corresponding class field) 
        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
           
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }
        // URI ends with "api/cities" since that was placed in Route param
        [HttpGet]
        // Want to return everything in JSON format 
        // Create constructor that returns JSON-ified data
        // name param = bound to query string (allows user to filter base on name) 
        // searchQuery param = used for searching 
        // pageNumber/pageSize = used for paging 
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities( [FromQuery]string? name, string? searchQuery, int pageNumber = 1, int pageSize= 10) 
        { 
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;   
            }


            // cityEntities = used by repository + context
            // GetCitiesAsync = returns a "tuple" (i.e. 2 variables instead of 1) 
            var (cityEntities, paginationMetadata) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            // Add pagination data to response as a header
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));



            //cityDto = used by API (therfore, use mapper to map: city enitity ("source object") -> city w/out POI Dto ("destination object") 
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));

        }

        [HttpGet("id/{id}")]
        // ActionResult = imported from ControllerBase ("action" = Http get/put/post)
        public async Task<IActionResult> GetCityById(int id, bool includePointsOfInterest = false) {

            // await keyword = gets result of task (rather than task itself) 
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                // Map Entities.City object -> Models.CityDto object
                return Ok(_mapper.Map<CityDto>(city));

            }

            // Map Entities.City -> Models.CityWithoutPointsOfInterestDto object
            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city)); 

        }

   

        // I made this all on my own hehe
        [HttpGet("cityname/{cityName}")]
        public ActionResult<CityDto> GetCityByName(string cityName) {

            return Ok(cityName);

            /*var cityReturned = _citiesDataStore.Cities.FirstOrDefault(city => city.Name == cityName);

            if (cityReturned == null)
            {
                return NotFound();
            }

            // Otherwise, return the object with a 200 status code
            return Ok(cityReturned);*/

            // If error occurs that is not handled, automatically returns 500 internal server error 
        }


     
    }
}
