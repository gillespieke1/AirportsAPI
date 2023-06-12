using AirportsAPI.Models;
using AirportsAPI.Repositories;
using Microsoft.AspNetCore.Routing;

namespace AirportsAPI.Services
{
    public class GeographyService : IGeographyService
    {
        public GeographyModel AddCountry(CreateGeographyRequest country)
        {
            // Generate new ID for new country
            int id = GetNewId();

            GeographyModel newCountry = new GeographyModel();
            
            //Check if country exists before adding it
            if(!CountryExists(country.Name))
            {
                //Assign values from input to newCountry object
                newCountry.GeographyLevel1Id = id;
                newCountry.Name = country.Name;

                //Add New Country
                GeographyRepository.Geographies.Add(newCountry);
                return newCountry;
            }

            return newCountry;
        }

        private static bool CountryExists(string newCountryName)
        {
            foreach(GeographyModel country in GeographyRepository.Geographies)
            {
                if((country.Name.ToUpper()).Contains(newCountryName.ToUpper()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Method to retrieve all countries and return to the user
        /// </summary>
        /// <returns></returns>
        public List<GeographyModel> GetAllCountries()
        {
            var countries = GeographyRepository.Geographies;

            return countries;
        }

        /// <summary>
        /// Method that deletes a country 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GeographyModel DeleteCountry(int id)
        {
            var countryToDelete = GeographyRepository.Geographies.Find(a => a.GeographyLevel1Id == id);

            if (countryToDelete != null)
            {
                var countryHasAirports = AirportRepository.Airports.Find
                       (a => a.GeographyLevel1Id == countryToDelete.GeographyLevel1Id);

                // Check if country has any airports
                if (countryHasAirports is null)
                {
                    GeographyRepository.Geographies.Remove(countryToDelete);
                    countryToDelete.ErrorCode = 200;
                }
                else
                {
                    countryToDelete.ErrorCode = 400;
                }
            }

            return countryToDelete;
        }

        /// <summary>
        /// Method to create a new ID for a newly added country
        /// </summary>
        private int GetNewId()
        {
            var newCountryId = 0;
            List<int> geographyIds = new List<int>();
            foreach(GeographyModel country in GeographyRepository.Geographies)
            {
                var currentId = country.GeographyLevel1Id;
                geographyIds.Add(currentId);
                if (currentId > newCountryId) newCountryId = currentId;
            }

            // increment to one higher than the largest ID in the existing list
            newCountryId++;
            return newCountryId;
        }
    }
}
