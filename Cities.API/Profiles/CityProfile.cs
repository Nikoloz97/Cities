using AutoMapper;

namespace Cities.API.Profiles
{
    public class CityProfile : Profile
    {
        // Mapping configs = set in constructor
        // Goal = map city entity -> city w/o POI DTO
        public CityProfile()
        {
            // If property doesn't exist = ignored 
            // Entities.City = "source object"
            // Modesl. CityWithout... = "destination object" 
            CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
                
        }
    }
}
