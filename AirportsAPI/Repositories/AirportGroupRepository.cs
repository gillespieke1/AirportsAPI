using AirportsAPI.Models;

namespace AirportsAPI.Repositories
{
    public class AirportGroupRepository
    {
        public static List<AirportGroupModel> Groups = new()
        {
            new() { AirportGroupId = 1, Name = "ArrivalAirportGroup" },
            new() { AirportGroupId = 2, Name = "DepartureAirportGroup" }
        };
    }
}
