using AirportsAPI.Models;

namespace AirportsAPI.Repositories
{
    public class RouteRepository
    {
        public static List<RouteModel> Routes = new()
        {
            new() { RouteId = 1, DepartureAirportId = 1 , ArrivalAirportId = 2, ArrivalAirportGroupId = null, DepartureAirportGroupId = null}
        };
    }
}
