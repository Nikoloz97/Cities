using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cities.API.Entities
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        // This attribute = not necessary (since navigation properties default with ID) 
        [ForeignKey("CityId")]
        // "Navigation property" (creates a relationship with city) 
        public City? City { get; set; }

        public PointOfInterest(string name)
        {
            this.Name = name;
        }
    }
}
