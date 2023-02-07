using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cities.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Cities.API.Services;

namespace Cities.API.Controllers
{
    [Route("api/cities/{cityId}/[controller]")]
    // This attribute = creates response messages
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        // Store depedency in a property
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly CitiesDataStore _citiesDataStore;

        // Injecting ILogger, mailservice (dependencies) into POI controller
        // Dependencies are all "registered" in the services container (see program) 
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, CitiesDataStore citiesDataStore)
        {
           
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); //the throw = "null check"
            _mailService = mailService  ?? throw new ArgumentNullException(nameof(mailService));
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            try
            {

            var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null)
            {
                // Documenting message that city wasn't found (puts it into console)
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing POI.");
                return NotFound();
            }
            return Ok(city.PointsOfInterest);

            }
            catch (Exception ex)
            {
                // Exceptions = always logged as "critical" level
                _logger.LogCritical($"Exception while getting POI for city with cityId {cityId}.", ex);
                // Unhandled exceptions = return 500 internal server error
                // Since we've caught it, must return this error manually (to the console)
                return StatusCode(500, "A problem happened while handling your request.");
            }


        }

        // We can name this route (second parameter)  
        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null) { return NotFound(); }

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
            if (city == null) { return NotFound(); }

            // From city, find corresponding POI
            var POI = city.PointsOfInterest.FirstOrDefault(POI => POI.Id == pointOfInterestId);
            if (POI == null) { return NotFound(); }

            // Map values into the model
            POI.Name = pointOfInterest.Name;
            POI.Description = pointOfInterest.Description;

            // Equivalent of returning a null (since put requests don't return anything)
            return NoContent();

        }

        // Patch = "partial update" of a resource
        [HttpPatch("{pointofinterestid}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
            if (city == null) { return NotFound(); }

            var POI = city.PointsOfInterest.FirstOrDefault(POI => POI.Id == pointOfInterestId);
            if (POI == null) { return NotFound(); }

            // Map POI to POI_ForUpdate
            var POI_Patch = new PointOfInterestForUpdateDto()
            {
                Name = POI.Name,
                Description = POI.Description,
            };

            // "Patch" the POI_Patch object
            // Allows you to manipulate objects (adding/removing properties) during runtime
            // ModelState = check for errors 
            patchDocument.ApplyTo(POI_Patch, ModelState);

            // Check if POI_Patch valid before applying patchDoc
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if POI_Patch valid after applying patchDoc
            if (!TryValidateModel(POI_Patch))
            {
                return BadRequest(ModelState);

            }

            // Map patched POI -> regular POI
            POI.Name = POI_Patch.Name;
            POI.Description = POI_Patch.Description;

            return NoContent();

        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
            if (city == null) { return NotFound();}

            var POI = city.PointsOfInterest.FirstOrDefault(POI => POI.Id == pointOfInterestId);
            if (POI == null) { return NotFound(); }

            city.PointsOfInterest.Remove(POI);
            _mailService.Send("POI Deleted", $"POI {POI.Name} with id {POI.Id} was deleted");
            return NoContent();

        }


    }
}
