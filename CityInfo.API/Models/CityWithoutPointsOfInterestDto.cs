namespace CityInfo.API.Models
{
    /// <summary>
    /// A DTO for a city without points of interest
    /// </summary>
    public class CityWithoutPointsOfInterestDto
    {
        public int Id { get; set; }
        /// <summary>
        /// The name of city
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The description of the city
        /// </summary>
        public string? Description { get; set; }

    }
}
