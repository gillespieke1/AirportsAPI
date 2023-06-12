using AirportsAPI.Models;

namespace AirportsAPI.Repositories
{
    public class GeographyRepository
    {
        public static List<GeographyModel> Geographies = new()
        {
            new() { GeographyLevel1Id = 1, Name = "United Kingdom"},
            new() { GeographyLevel1Id = 2, Name = "Spain"},
            new() { GeographyLevel1Id = 3, Name = "United States"},
            new() { GeographyLevel1Id = 4, Name = "Turkey"}
        };  
    }
}
