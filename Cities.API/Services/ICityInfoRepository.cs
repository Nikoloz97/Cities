using Cities.API.Entities;

namespace Cities.API.Services
{
    public interface ICityInfoRepository
    {
        // Async function = allows for app scalability
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityAsync(int cityId, bool includePOI);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);

    }
}
