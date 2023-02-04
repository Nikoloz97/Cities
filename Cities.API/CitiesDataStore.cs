using Cities.API.Models;

namespace Cities.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }   

        // "Current" property = returns instance of the CitiesDataStore (i.e. calls on the constructor) 
        // Static, so CitiesDataStore doesn't need to be instantiated to access this property
        public static CitiesDataStore Current { get; } = new CitiesDataStore();


        // Constructor = contains "dummy data"
        public CitiesDataStore() 
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with the big park.",
                     PointsOfInterest = new List<PointOfInterestDto>()
                     {
                         new PointOfInterestDto() {
                             Id = 1,
                             Name = "Central Park",
                             Description = "The most visited urban park in the United States." },
                          new PointOfInterestDto() {
                             Id = 2,
                             Name = "Empire State Building",
                             Description = "A 102-story skyscraper located in Midtown Manhattan." },
                     }
                },

                new CityDto()
                {
                    Id = 2,
                    Name = "Berlin",
                    Description = "The capital of the axis in WW2.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                     {
                         new PointOfInterestDto() {
                             Id = 3,
                             Name = "Cathedral of Our Lady",
                             Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans." },
                          new PointOfInterestDto() {
                             Id = 4,
                             Name = "Antwerp Central Station",
                             Description = "The the finest example of railway architecture in Belgium." },
                     }
                },

                new CityDto()
                {
                    Id = 3,
                    Name = "Tokyo",
                    Description = "The asian capital of the axis.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                     {
                         new PointOfInterestDto() {
                             Id = 5,
                             Name = "Cathedral of Our Lady",
                             Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans." },
                          new PointOfInterestDto() {
                             Id = 6,
                             Name = "Antwerp Central Station",
                             Description = "The the finest example of railway architecture in Belgium." },
                     }
                }

            };
        }

    }
}
