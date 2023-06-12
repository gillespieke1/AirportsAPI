using AirportsAPI;
using AirportsAPI.Models;
using AirportsAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<IAirportService, AirportService>();
builder.Services.AddSingleton<IRouteService, RouteService>();
builder.Services.AddSingleton<IGeographyService, GeographyService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Airports Endpoints


//Get single airport based on ID given
app.MapGet("/airports/{id}", (int airportId, IAirportService service) =>
{
    var airport = service.GetAirport(airportId);

    if (airport is null) return Results.NotFound("Airport not found.");

    return Results.Ok(airport);
})
.WithName("GetAirport");

//Get all airports/*
app.MapGet("/airports", (IAirportService service) =>
{
    var airports = service.GetAllAirports().ToList();
    return Results.Ok(airports);
})
.WithName("GetAllAirports");


// Country/Geography Endpoints


//Add a country
app.MapPost("/countries", ([FromBody]CreateGeographyRequest country, IGeographyService service) =>
{
    var result = service.AddCountry(country);

    if (result.Name is null) return Results.Text("Country already exists.");

    return Results.Ok(result);
})
.WithName("AddCountry");

//Get all countries/*
app.MapGet("/countries", (IGeographyService service) =>
{
    var countries = service.GetAllCountries();

    return Results.Ok(countries);
})
.WithName("GetAllCountries");

//Delete a country
app.MapDelete("/countries/{id}", (int id, IGeographyService service) =>
{
    var result = service.DeleteCountry(id);

    if(result is null) return Results.NotFound("Country not found.");
    if (result.ErrorCode == 400) return Results.BadRequest("Country is in use and cannot be deleted.");

    return Results.Ok();
})
.WithName("DeleteCountry");


// Route Endpoints


//Add a route
app.MapPost("/routes", ([FromBody] CreateRouteRequest route, IRouteService service) =>
{
    RouteModel result;
    
    // If no information is provided, return bad request
    if(route.ArrivalAirportGroupId == 0 && route.DepartureAirportId == 0 && route.ArrivalAirportGroupId == 0 && route.DepartureAirportGroupId == 0)
    {
        return Results.BadRequest();
    }
    // If airport IDs are provided (no group IDs)
    if (route.ArrivalAirportId != 0 && route.DepartureAirportId != 0)
    {
        result = service.AddRoute(route);

        if (result.ErrorCode == 404) return Results.NotFound(result.ErrorMessage);
        if (result.ErrorCode == 400) return Results.BadRequest(result.ErrorMessage);
    }
    // If airport group IDs are provided 
    else
    {
        result = service.AddRouteWithGroups(route);

        if (result.ErrorCode == 404) return Results.NotFound(result.ErrorMessage);
        if (result.ErrorCode == 400) return Results.BadRequest(result.ErrorMessage);
    }

    return Results.Ok(result);
})
.WithName("AddRoute");

//Get all routes/*
app.MapGet("/routes", (IRouteService service) =>
{
    var routes = service.GetAllRoutes();

    return Results.Ok(routes);
})
.WithName("GetAllRoutes");


app.Run();

