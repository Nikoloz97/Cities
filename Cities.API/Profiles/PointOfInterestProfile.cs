using AutoMapper;

namespace Cities.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {

        public PointOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();

        }
    }
}
