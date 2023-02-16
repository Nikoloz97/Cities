using Cities.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cities.API.DBContexts
{
    // DbContexts = "bridge between entity classes and the database" 
    // DbContext = class inherited from Entity Framework
    public class CityInfoContext : DbContext
    {
        // Dbset = creates instances of its entity type, City (see Entities.City) 
        public DbSet<City> Cities { get; set; } = null!;

        // DbContext doesn't allow for nulls, so include "null forgiver" (null!)
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        // Constructor purpose = provide options at moment DBContext is registered 
        // See Services.AddDbContext (program.cs) 
        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        { 

        }

        // Gain access to modelbuilder (purpose = manually construct models)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // HasData = passes in mock data ("seeding DB")
            modelBuilder.Entity<City>()
                .HasData(
               new City("New York City")
               {
                   Id = 1,
                   Description = "The one with that big park."
               },
               new City("Antwerp")
               {
                   Id = 2,
                   Description = "The one with the cathedral that was never really finished."
               },
               new City("Paris")
               {
                   Id = 3,
                   Description = "The one with that big tower."
               });

            modelBuilder.Entity<PointOfInterest>()
             .HasData(
               new PointOfInterest("Central Park")
               {
                   Id = 1,
                   CityId = 1,
                   Description = "The most visited urban park in the United States."
               },
               new PointOfInterest("Empire State Building")
               {
                   Id = 2,
                   CityId = 1,
                   Description = "A 102-story skyscraper located in Midtown Manhattan."
               },
                 new PointOfInterest("Cathedral")
                 {
                     Id = 3,
                     CityId = 2,
                     Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
                 },
               new PointOfInterest("Antwerp Central Station")
               {
                   Id = 4,
                   CityId = 2,
                   Description = "The the finest example of railway architecture in Belgium."
               },
               new PointOfInterest("Eiffel Tower")
               {
                   Id = 5,
                   CityId = 3,
                   Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel."
               },
               new PointOfInterest("The Louvre")
               {
                   Id = 6,
                   CityId = 3,
                   Description = "The world's largest museum."
               }
               );



            base.OnModelCreating(modelBuilder);

        }
        
    }
}
