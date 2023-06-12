using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AirportsAPI.Models
{
    public class GeographyModel
    {
        public int GeographyLevel1Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public int ErrorCode { get; set; }
    }
}
