namespace Cities.API.Models
{
    public class PointOfInterestDto
    {
        // DTO = data transfer object
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

       
    }
}
