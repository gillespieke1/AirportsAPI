using AirportsAPI.Models;
using AirportsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportsAPITests
{
    public class CountriesTests
    {
        public GeographyService service = new GeographyService();
        CreateGeographyRequest countryThatExists = new CreateGeographyRequest()
        {
            Name = "Spain",
        };

        CreateGeographyRequest newCountry = new CreateGeographyRequest()
        {
            Name = "Portugal"
        };

        [Fact]
        public void AddCountryThatExists()
        {
            GeographyModel country = service.AddCountry(countryThatExists);

            Assert.Null(country.Name);
        }

        [Fact]
        public void AddNewCountry()
        {
            GeographyModel country = service.AddCountry(newCountry);

            Assert.NotNull(country);
        }

        [Fact]
        public void GetAllCountries()
        {
            List<GeographyModel> countries = service.GetAllCountries();

            Assert.True(countries.Count == 4);
        }
    }
}
