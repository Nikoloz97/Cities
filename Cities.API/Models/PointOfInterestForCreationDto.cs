namespace Cities.API.Models
{
    public class PointOfInterestForCreationDto
    {
        // Rule of thumb = create separate models for creating, updating, and returning resources

        // This is a model that only contains properties that user can specify (i.e. no ID)

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }


    }
}
