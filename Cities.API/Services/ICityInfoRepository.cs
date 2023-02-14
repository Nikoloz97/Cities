using Cities.API.Entities;

namespace Cities.API.Services
{
    // These are known as "signatures"
    public interface ICityInfoRepository
    {
        // Async function = allows for app scalability
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityAsync(int cityId, bool includePOI);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
        Task<bool> CityExistsAsync(int cityId);
        // Adding = NOT an async method (however, since need to get the POI before adding, then by technically it is Async) 
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);

        Task<bool> SaveChangesAsync ();

        // Deleting = NOT an async method
        void DeletePointOfInterestForCity(PointOfInterest pointOfInterest);


    }
}
