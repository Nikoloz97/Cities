using Cities.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cities.API.DBContexts
{
    // DbContext = class inherited from Entity Framework
    public class CityInfoContext : DbContext
    {
        // Dbset = creates instances of its entity type (translated to queries in DB) 
        public DbSet<City> Cities { get; set; } = null!;

        // DbContext doesn't allow for nulls, so include "null forgiver" (null!)
        public DbSet<PointOfInterest> PointOfInterest { get; set; } = null!;

        // Constructor purpose = provide options at moment DBContext is registered 
        // See Services.AddDbContext (program.cs) 
        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        { 

        }
        
    }
}
