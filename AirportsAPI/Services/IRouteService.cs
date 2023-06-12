using AirportsAPI.Models;

namespace AirportsAPI.Services
{
    public interface IRouteService
    {
        public List<RouteModel> GetAllRoutes();
        public RouteModel AddRoute(CreateRouteRequest route);
        public RouteModel AddRouteWithGroups(CreateRouteRequest route);
    }
}
