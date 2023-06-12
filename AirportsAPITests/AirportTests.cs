using AirportsAPI.Models;
using AirportsAPI.Services;

namespace AirportsAPITests
{
    public class AirportsTests
    {
        public AirportService service = new AirportService();

        AirportModel airportToRetrieve = new AirportModel()
        {
            AirportId = 1,
            IATACode = "LGW",
            GeographyLevel1Id = 1,
            Type = "Departure and Arrival"
        };

        AirportModel airportNotFound = new AirportModel()
        {
            AirportId = 12
        };

        [Fact]
        public void AirportsTestById()
        {
            AirportModel airport = service.GetAirport(airportToRetrieve.AirportId);

            Assert.True(airport.AirportId == airportToRetrieve.AirportId);
            Assert.True(airport.GeographyLevel1Id == airportToRetrieve.GeographyLevel1Id);
            Assert.Matches(airport.IATACode, airportToRetrieve.IATACode);
            Assert.Matches(airport.Type, airportToRetrieve.Type);
        }

        [Fact]
        public void AirportTestsReturnNotFound()
        {
            AirportModel airport = service.GetAirport(airportNotFound.AirportId);

            Assert.Null(airport);
        }
    }
}