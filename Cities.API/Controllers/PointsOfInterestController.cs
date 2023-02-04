using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cities.API.Models;

namespace Cities.API.Controllers
{
    [Route("api/cities/{cityId}/[controller]")]
    // This attribute = creates response messages
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

        // We can name this route (second parameter)  
        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null) { return NotFound();}

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(interest => interest.Id == pointOfInterestId);
            

            if (pointOfInterest == null) { return NotFound(); }

            return Ok(pointOfInterest);

        }

        [HttpPost]
        // Since parameters are multiple, this is a complex type 
        // From body attribute = assumed by program (putting that on there is optional)
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityID, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            // Find matching cityId 
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id.Equals(cityID));

            if (city == null) { return NotFound(); }

            // Mapping out each city's POI object aray, grab the max Id 
            var maxPOI_Id = CitiesDataStore.Current.Cities.SelectMany(city => city.PointsOfInterest).Max(city => city.Id);

            // Map POI creation dto -> POI dto 
            // "Mapping" = transferring info of one object (POI_creation) -> to another object (POI)
            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPOI_Id,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(finalPointOfInterest);

            // Param 1 = points to previous Get request
            // Param 2 = map params of that Get request to this Post request 
            // Param 3 = new PointOfInterest object
            // Return 200 status code if route was made for this new PointOfInterest object
            return CreatedAtRoute("GetPointOfInterest",
                new 
                {
                    cityId = cityID,
                    pointOfInterestId = finalPointOfInterest.Id
                },
                finalPointOfInterest);

        }

        [HttpPut("{pointofinterestid}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestDto pointOfInterest) 
        {
            // Get corresponding city
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
            if (city == null) { return NotFound();}

            // From city, find corresponding POI
            var POI = city.PointsOfInterest.FirstOrDefault(POI => POI.Id == pointOfInterestId);
            if (POI == null) { return NotFound(); }

            // Map values into the model
            POI.Name = pointOfInterest.Name;
            POI.Description = pointOfInterest.Description;

            // Equivalent of returning a null
            return NoContent();
            
        }

    }
}
