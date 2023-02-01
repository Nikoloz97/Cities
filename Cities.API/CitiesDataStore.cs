using Cities.API.Models;

namespace Cities.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }   

        // "Current" property = returns instance of the CitiesDataStore
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
                    Description = "The one with the big park."
                },

                new CityDto()
                {
                    Id = 2,
                    Name = "Berlin",
                    Description = "The capital of the axis in WW2."
                },

                new CityDto()
                {
                    Id = 3,
                    Name = "Tokyo",
                    Description = "The asian capital of the axis."
                }

            };
        }

    }
}
