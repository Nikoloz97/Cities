using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cities.API.Entities
{
    // Entities = similar to corresponding models (i.e. CityDto)
    public class City
    {

        [Key]
        // Automatic key generation when new city added
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        // Initialize to empty list to avoid null reference exception
        public ICollection<PointOfInterest> PointsOfInterest { get; set; } 
            = new List<PointOfInterest>();

        // Makes the name non-nullable 
        // Conveys a message to other developers (i.e. city class should ALWAYS have a name) 
        public City(string name)
        {
            this.Name = name;

        }
    }
}
