namespace AirportsAPI.Models
{
    public class CreateRouteRequest
    {
        public int DepartureAirportId { get; set; }
        public int ArrivalAirportId { get; set; }
        public int ArrivalAirportGroupId { get; set; }
        public int DepartureAirportGroupId { get; set; }
    }
}
