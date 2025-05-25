using CleanArch.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArch.Web.Endpoints;

public class WeatherForecasts : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this).MapGet(GetWeatherForecasts);
    }

    public async Task<Ok<IEnumerable<WeatherForecast>>> GetWeatherForecasts(ISender sender)
    {
        var forecasts = await sender.Send(new GetWeatherForecastsQuery());

        return TypedResults.Ok(forecasts);
    }
}
