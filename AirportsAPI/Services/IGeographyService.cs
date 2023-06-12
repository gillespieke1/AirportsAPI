using AirportsAPI.Models;

namespace AirportsAPI.Services
{
    public interface IGeographyService
    {
        public List<GeographyModel> GetAllCountries();
        public GeographyModel AddCountry(CreateGeographyRequest country);
        public GeographyModel DeleteCountry(int id);
    }
}
