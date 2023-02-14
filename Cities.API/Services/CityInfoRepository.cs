using Cities.API.DBContexts;
using Cities.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cities.API.Services
{
    // For repository pattern, this is where "persistance logic" is created
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext context;

        // Dependency inject CityInfoContext
        public CityInfoRepository(CityInfoContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
         
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            // Order by = "additional persistance logic" (i.e. something extra not hinted in our interface)
            return await this.context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePOI)
        {
            if (includePOI)
            {
                // Include POI when getting city 
                // FirstOrDefaultAsync = "executes query" 
                return await this.context.Cities.Include(c => c.PointsOfInterest).Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }

            return await this.context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();

        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await this.context.PointsOfInterest.Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            // ToListAsync = returns list 
            return await this.context.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();
        }
    }
}
