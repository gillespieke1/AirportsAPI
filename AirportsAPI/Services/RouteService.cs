using AirportsAPI.Models;
using AirportsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Runtime.Intrinsics.Arm;

namespace AirportsAPI.Services
{
    public class RouteService : IRouteService
    {
        public int errorCode = 0;
        public string errorMessage = string.Empty;
        public bool useGroupIds = false;

        public List<RouteModel> GetAllRoutes()
        {
            var routes = RouteRepository.Routes;

            return routes;
        }

        public RouteModel AddRoute(CreateRouteRequest route)
        {
            RouteModel newRoute = new();

            bool doesRouteExist = DoesRouteExist(route);
            bool isRouteValid = RouteIsValid(route);

            // Check if route already exists
            if(doesRouteExist)
            {
                newRoute.ErrorCode = 400;
                newRoute.ErrorMessage = "Route already exists.";
            }
            // Check if Route already exists and if route/airports in route are valid
            else if (!doesRouteExist && isRouteValid)
            {
                // Add check to see if RouteId exists already before assigning
                newRoute.RouteId = RouteRepository.Routes.Count + 1;
                newRoute.ArrivalAirportId = route.ArrivalAirportId;
                newRoute.DepartureAirportId = route.DepartureAirportId;

                RouteRepository.Routes.Add(newRoute);
            }
            else
            {
                newRoute.ErrorCode = errorCode;
                newRoute.ErrorMessage = errorMessage;
            }

            return newRoute;
        }

        public RouteModel AddRouteWithGroups(CreateRouteRequest route)
        {
            useGroupIds = true;
            RouteModel newRoute = new();

            // Check if Route already exists and if groups in route are valid
            if (!DoesRouteExist(route) && AirportGroupsAreValid(route))
            {
                // Add check to see if RouteId exists already before assigning
                newRoute.RouteId = RouteRepository.Routes.Count + 1;
                newRoute.ArrivalAirportGroupId = route.ArrivalAirportGroupId;
                newRoute.DepartureAirportGroupId = route.DepartureAirportGroupId;

                RouteRepository.Routes.Add(newRoute);
            }
            else
            {
                newRoute.ErrorCode = errorCode;
                newRoute.ErrorMessage = errorMessage;
            }

            return newRoute;
        }

        private bool AirportGroupsAreValid(CreateRouteRequest route)
        {
            // Using airport groups instead of single airports
            List<AirportModel> airportsInArrivalsGroup = new();
            List<AirportModel> airportsInDeparturesGroup = new();

            // Create two bools to check later if both are valid and handle accordingly
            var arrivalsGroupIsValid = false;
            var departuresGroupIsValid = false;

            //Check if there are any airports in the provided arrivals group & that the group exists
            if (AirportRepository.Airports.Where(a => a.AirportGroups.Contains(route.ArrivalAirportGroupId)) != null
                        && AirportGroupRepository.Groups.Find(g => g.AirportGroupId == route.ArrivalAirportGroupId) != null)
            {
                airportsInArrivalsGroup.AddRange(AirportRepository.Airports.Where(a => a.AirportGroups.Contains(route.ArrivalAirportGroupId)));
            }
            else
            {
                // No airports belong to the provided airport group - return Not Found
                errorCode = 404;
            }

            // Check if there are any airports in the provided departures group & that group exists
            if (AirportRepository.Airports.Where(a => a.AirportGroups.Contains(route.DepartureAirportGroupId)) != null 
                && AirportGroupRepository.Groups.Find(g => g.AirportGroupId == route.DepartureAirportGroupId) != null)
            {
                airportsInDeparturesGroup.AddRange(AirportRepository.Airports.Where(a => a.AirportGroups.Contains(route.DepartureAirportGroupId)));
            }
            else
            {
                // No airports belong to the provided airport group - return Not Found
                errorCode = 404;
                errorMessage = "Airport group not found";
            }

            // If there are airports in each group, check to make sure they are valid for the arrivals or departures group
            if(airportsInArrivalsGroup.Count > 0 && airportsInDeparturesGroup.Count > 0)
            {

                foreach (AirportModel airport in airportsInArrivalsGroup)
                {
                    if (!airport.Type.Contains("Arrival"))
                    {
                        errorCode = 400;
                    }
                }
                arrivalsGroupIsValid = true;

                foreach (AirportModel airport in airportsInDeparturesGroup)
                {
                    if (!airport.Type.Contains("Departure"))
                    {
                        errorCode = 400;
                    }
                }
                departuresGroupIsValid = true;
            }

            if (departuresGroupIsValid && arrivalsGroupIsValid) return true;
            
            // Set appropriate error message
            if (airportsInArrivalsGroup is null && airportsInDeparturesGroup is null) 
                errorMessage = "Arrival and Departure airport groups could not be found.";
            if (airportsInArrivalsGroup is null && airportsInDeparturesGroup != null) 
                errorMessage = "Arrivals airport group could not be found.";
            if (airportsInArrivalsGroup != null && airportsInDeparturesGroup is null) 
                errorMessage = "Departures airport group could not be found.";
            if (!arrivalsGroupIsValid || !departuresGroupIsValid)
                errorCode = 400;
                errorMessage = "This route is not valid.";

            return false;
        }

        private bool RouteIsValid(CreateRouteRequest route)
        {
            var arrivalAirport = AirportRepository.Airports.Find(a => a.AirportId == route.ArrivalAirportId);
            var departureAirport = AirportRepository.Airports.Find(d => d.AirportId == route.DepartureAirportId);

            // Check that arrival and departure airports exist
            if (arrivalAirport != null && departureAirport != null)
            {
                // Check if airports are valid for route
                if (arrivalAirport.Type.Contains("Arrival") && departureAirport.Type.Contains("Departure"))
                {
                    return true;
                }
                // else if airports provided are not valid for arrival or departure, error code will be set to bad request
                else
                {
                    errorMessage = "Route is not valid.";
                    errorCode = 400;
                }
            }
            // else if one or both airports provided do not exist, error code will be set to Not Found
            else
            {
                if (arrivalAirport == null && departureAirport == null) errorMessage = "Arrival and Departure airports could not be found.";
                else if (arrivalAirport == null && departureAirport != null) errorMessage = "Arrival airport could not be found.";
                else errorMessage = "Departure airport could not be found.";
                errorCode = 404;

            }
           
            return false;
        }

        private bool DoesRouteExist(CreateRouteRequest newRoute)
        {
            if (!useGroupIds)
            {
               foreach (RouteModel route in RouteRepository.Routes)
                {
                    // Check if both DepartureAirportId and ArrivalAirportId are same as any existing routes
                    if (route.DepartureAirportId == newRoute.DepartureAirportId
                        && route.ArrivalAirportId == newRoute.ArrivalAirportId)
                        return true;
                } 
            }
            else
            {
                foreach (RouteModel route in RouteRepository.Routes)
                {
                    // Check if both DepartureAirportId and ArrivalAirportId are same as any existing routes
                    if (route.DepartureAirportGroupId == newRoute.DepartureAirportGroupId
                        && route.ArrivalAirportGroupId == newRoute.ArrivalAirportGroupId)
                        return true; 
                }
            }
            return false;
        }
    }
}
