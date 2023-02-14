using Cities.API.DBContexts;
using Cities.API.Entities;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

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

        public async Task<bool> CityExistsAsync(int cityId)
        {
            // Any = if finds at least one occurance, returns true 
            return await context.Cities.AnyAsync(c => c.Id == cityId);
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


        // Post = not returning anything
        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);
            if (city != null)
            {
                // Not AddAsync since doesn't go to database (instead, adds it to "object context") 
                city.PointsOfInterest.Add(pointOfInterest);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            // Returns number of changes that have been saved  
            return (await this.context.SaveChangesAsync() >= 0);
        }
    }
}
