using AirportsAPI.Models;

namespace AirportsAPI.Repositories
{
    public class AirportRepository
    {
        public static List<AirportModel> Airports = new()
        {
            new() { AirportId = 1, IATACode = "LGW" ,GeographyLevel1Id = 1, Type = "Departure and Arrival", AirportGroups = new List<int>() { 1, 2} },
            new() { AirportId = 2, IATACode = "PMI", GeographyLevel1Id = 2, Type = "Arrival Only", AirportGroups = new List<int>() { 1 } },
            new() { AirportId = 3, IATACode = "LAX", GeographyLevel1Id = 3, Type = "Arrival Only", AirportGroups = new List<int>() { 1 } }
        };
    }
}
