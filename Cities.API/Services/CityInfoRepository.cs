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

        // Overload of GetCitiesAsync
        // Used for filtering based on city name (which is bound to query string param)
        // Returning IEnumerable City AND pagination = "tuple" 

        public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
        {

            // "Cast" DBSet -> IQueryable <City> (allows us to use linq clauses like where, etc.) 
            // Known as "deferred execution" (aka we're "building up" the query, and only sending to database once we call ToListAsync below) 
            var collection = this.context.Cities as IQueryable<City>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }

            // Searches = more broad than filter
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(a => a.Name.Contains(searchQuery) || (a.Description != null && a.Description.Contains(searchQuery)));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);


            // Adding paging functionality last = good practice (otherwise query executes on the individual page data rather than whole thing) 
            // Skip = e.g. request page 2 -> skips content from page 1 (if request page 1, doesn't skip anything) 
            var collectionToReturn =  await collection.OrderBy(c => c.Name).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();

            // Return a "tuple" 
            return (collectionToReturn, paginationMetadata);

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

        public void DeletePointOfInterestForCity(PointOfInterest pointOfInterest)
        {
            this.context.Remove(pointOfInterest);
        }
    }
}
