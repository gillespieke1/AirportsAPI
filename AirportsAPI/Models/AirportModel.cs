namespace AirportsAPI.Models
{
    public class AirportModel
    {
        public int AirportId { get; set; }
        public string IATACode { get; set; }
        public int GeographyLevel1Id { get; set; }
        public string Type { get; set; }
        // A list of Airport Group IDs the airport belongs to
        public List<int> AirportGroups { get; set; }
    }
}
