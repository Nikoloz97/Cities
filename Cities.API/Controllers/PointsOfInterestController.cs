using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cities.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Cities.API.Services;
using AutoMapper;

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
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        // Injecting ILogger, mailservice (dependencies) into POI controller
        // Dependencies are all "registered" in the services container (see program) 
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
           
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); //the throw = "null check"
            _mailService = mailService  ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));    
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} was not found.");
                return NotFound();

            }

            var pointsOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
          

            /*
            
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
            
             */
        }

        // We can name this route (second parameter)  
        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"The city by Id {cityId} could not be found");
                return NotFound();
            }

            var pointOfInterestForCity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestForCity == null)
            {
                _logger.LogInformation($"The POI by Id {pointOfInterestId} could not be found");
                return NotFound();

            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterestForCity));


            /*
             
            var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null) { return NotFound(); }

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(interest => interest.Id == pointOfInterestId);


            if (pointOfInterest == null) { return NotFound(); }

            return Ok(pointOfInterest);
            
             */

        }

        [HttpPost]
        // Since parameters are multiple, known as a "complex type" 
        // From body attribute = assumed by program (putting that on there is optional)
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityID, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityID))
            {
                return NotFound();
            }

            // 1.Store map creation-DTO -> Entity
            var newPOI = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            // 2. Call Repository function
            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityID, newPOI);

            // 3. "Persist the changes" (aka data isn't lost) 
            await _cityInfoRepository.SaveChangesAsync();

            // 4. Map the stored map (final POI) -> regular-DTO 
            var createdPOIToReturn = _mapper.Map<Models.PointOfInterestDto>(newPOI);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityID,
                    pointOfInterestId = createdPOIToReturn.Id
                },
                createdPOIToReturn);




            /*
             
             // Find matching cityId 
            var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id.Equals(cityID));

            if (city == null) { return NotFound(); }

            // Mapping out each city's POI object aray, grab the max Id 
            var maxPOI_Id = _citiesDataStore.Cities.SelectMany(city => city.PointsOfInterest).Max(city => city.Id);

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

            */

        }


        // pointOfInterest param = "source body" 
        [HttpPut("{pointofinterestid}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
        {

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound() ;
            }

            var PoiEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (PoiEntity == null)
            {
                return NotFound();
            }

             // 1. Special update-Map function = overrides destination object (2nd param) with source object (1st param)
            _mapper.Map(pointOfInterest, PoiEntity);

            // 2. Persist changes
            await _cityInfoRepository.SaveChangesAsync();


             return NoContent();



/*
            // Get corresponding city
            var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);
            if (city == null) { return NotFound(); }

            // From city, find corresponding POI
            var POI = city.PointsOfInterest.FirstOrDefault(POI => POI.Id == pointOfInterestId);
            if (POI == null) { return NotFound(); }

            // Map values into the model
            POI.Name = pointOfInterest.Name;
            POI.Description = pointOfInterest.Description;

            // Equivalent of returning a null (since put requests don't return anything)
            return NoContent();

*/

        }

        

        // Patch = "partial update" of a resource (preferred over full updates since quicker) 
        [HttpPatch("{pointofinterestid}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            };

            // 1. Find POI entity
            var PoiEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (PoiEntity == null)
            {
                return NotFound();
            };

            // 2. Map POI entity -> update-DTO (store mapped update-DTO in variable) 
            var POIToPatch =  _mapper.Map<PointOfInterestForUpdateDto>(PoiEntity);

        
            // 3. Apply the "Patch" to the POI_Patch object (allows you to manipulate objects (adding/removing properties) during runtime)
            // ModelState = check for errors 
            patchDocument.ApplyTo(POIToPatch, ModelState);

            // 4. Check if POI_Patch valid before applying patchDoc
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 5. Check if POI_Patch valid after applying patchDoc
            if (!TryValidateModel(POIToPatch))
            {
                return BadRequest(ModelState);

            }

            // 6. Map changes back to entity
            _mapper.Map(POIToPatch, PoiEntity);

            // 7. Save changes
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent(); 


            /*
             
            var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);
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
            
             */

        }

        /*

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);
            if (city == null) { return NotFound();}

            var POI = city.PointsOfInterest.FirstOrDefault(POI => POI.Id == pointOfInterestId);
            if (POI == null) { return NotFound(); }

            city.PointsOfInterest.Remove(POI);
            _mailService.Send("POI Deleted", $"POI {POI.Name} with id {POI.Id} was deleted");
            return NoContent();

        }
*/

    }
}
