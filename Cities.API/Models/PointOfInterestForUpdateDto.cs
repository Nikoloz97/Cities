using System.ComponentModel.DataAnnotations;

namespace Cities.API.Models
{
    public class PointOfInterestForUpdateDto
    {
        [Required(ErrorMessage = "You should provide a name value.")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
