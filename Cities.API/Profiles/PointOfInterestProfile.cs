using AutoMapper;

namespace Cities.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {

        public PointOfInterestProfile()
        {
            // Mapping entities -> models = Get/Patch
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();


            // Mapping models -> entities = Post/Put
            CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
            CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();
            
        }
    }
}
