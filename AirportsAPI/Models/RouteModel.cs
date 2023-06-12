using System.Text.Json.Serialization;

namespace AirportsAPI.Models
{
    public class RouteModel
    {
        public int RouteId { get; set; }
        public int? DepartureAirportId { get; set; }
        public int? ArrivalAirportId { get; set; }
        public int? ArrivalAirportGroupId { get; set; }
        public int? DepartureAirportGroupId { get; set; }
        [JsonIgnore]
        public int ErrorCode { get; set; }
        [JsonIgnore]
        public string ErrorMessage { get; set; }
    }
}
