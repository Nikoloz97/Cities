using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cities.API.Models;

namespace Cities.API.Controllers
{
    [Route("api/cities/{cityId}/[controller]")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);


            if (city == null)
            {
                return NotFound();
            }
            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{pointOfInterestId}")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null) { return NotFound();}

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(interest => interest.Id == pointOfInterestId);
            

            if (pointOfInterest == null) { return NotFound(); }

            return Ok(pointOfInterest);

        }
    }
}
