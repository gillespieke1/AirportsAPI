using AirportsAPI.Models;
using AirportsAPI.Services;

namespace AirportsAPITests
{
    public class RouteTests
    {
        public RouteService service = new RouteService();

        CreateRouteRequest routeToAdd = new CreateRouteRequest()
        {
            ArrivalAirportId = 3,
            DepartureAirportId = 1,
            ArrivalAirportGroupId = 0,
            DepartureAirportGroupId = 0
        };

        CreateRouteRequest existingRoute = new CreateRouteRequest()
        {
            ArrivalAirportId = 2,
            DepartureAirportId = 1,
            ArrivalAirportGroupId = 0,
            DepartureAirportGroupId = 0
        };

        CreateRouteRequest routeNotFound = new CreateRouteRequest()
        {
            ArrivalAirportId = 12,
            DepartureAirportId = 1,
            ArrivalAirportGroupId = 0,
            DepartureAirportGroupId = 0
        };

        CreateRouteRequest invalidRoute = new CreateRouteRequest()
        {
            ArrivalAirportId = 1,
            DepartureAirportId = 2,
            ArrivalAirportGroupId = 0,
            DepartureAirportGroupId = 0
        };

        CreateRouteRequest validAirportGroupRoute = new CreateRouteRequest()
        {
            ArrivalAirportId = 0,
            DepartureAirportId = 0,
            ArrivalAirportGroupId = 1,
            DepartureAirportGroupId = 2
        };

        CreateRouteRequest invalidAirportGroupRoute = new CreateRouteRequest()
        {
            ArrivalAirportId = 0,
            DepartureAirportId = 0,
            ArrivalAirportGroupId = 1,
            DepartureAirportGroupId = 3
        };

        [Fact]
        public void GetAllRoute()
        {
            List<RouteModel> routes = service.GetAllRoutes();
            Assert.True(routes.Count == 1);
        }

        [Fact]
        public void AddRouteAlreadyExists()
        {
            RouteModel route = service.AddRoute(existingRoute);
            Assert.True(route.ErrorCode == 400);
            Assert.True(service.GetAllRoutes().Count == 1);
            Assert.Contains(route.ErrorMessage, "Route already exists.");
        }

        [Fact]
        public void AddNewRoute()
        {             
            RouteModel route = service.AddRoute(routeToAdd);
            Assert.True(route.ErrorCode == 0);
        }

        [Fact]
        public void AddInvalidRoute()
        {
            RouteModel route = service.AddRoute(invalidRoute);

            Assert.True(route.ErrorCode == 400);
            Assert.Contains(route.ErrorMessage, "Route is not valid.");
        }

        [Fact]
        public void AddRouteArrivalAirportNotFound()
        {
            RouteModel route = service.AddRoute(routeNotFound);

            Assert.True(route.ErrorCode == 404);
        }

        [Fact]
        public void AddRouteForRouteGroup()
        {
            RouteModel route = service.AddRouteWithGroups(validAirportGroupRoute);

            Assert.True(route.ErrorCode == 0);
        }

        [Fact]
        public void AddRouteForInvalidRouteGroup()
        {
            RouteModel route = service.AddRouteWithGroups(invalidAirportGroupRoute);

            Assert.True(route.ErrorCode == 400);
        }
    }
}