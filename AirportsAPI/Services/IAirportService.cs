using AirportsAPI.Models;

namespace AirportsAPI.Services
{
    public interface IAirportService
    {
        public List<AllAirportsResponse> GetAllAirports();
        public AirportModel GetAirport(int id);
    }
}
