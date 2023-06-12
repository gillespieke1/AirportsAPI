using AirportsAPI.Models;
using AirportsAPI.Repositories;

namespace AirportsAPI.Services
{
    public class AirportService : IAirportService
    {
        public AirportModel? GetAirport(int id)
        {
            var airport = AirportRepository.Airports.FirstOrDefault(a => a.AirportId == id);
          
            if (airport is null) 
                return null;
            
            return airport;
        }

        public List<AllAirportsResponse> GetAllAirports()
        {
            List<AllAirportsResponse> airports = new();
            foreach (AirportModel airport in AirportRepository.Airports)
            {
                airports.Add(
                        new() { AirportId = airport.AirportId, IATACode = airport.IATACode });
            }

            return airports;
        }
    }
}
