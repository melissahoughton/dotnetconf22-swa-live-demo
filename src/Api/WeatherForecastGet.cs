using System.Net;
using Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Api;

public class WeatherForecastGet
{
    private readonly ILogger _logger;

    public WeatherForecastGet(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<WeatherForecastGet>();
    }

    [Function("WeatherForecastGet")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var response = req.CreateResponse(HttpStatusCode.OK);

        var randomNumber = new Random();
        var temp = 0;

        var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = temp = randomNumber.Next(-10, 35),
            Summary = GetSummary(temp)
        }).ToArray();

        await response.WriteAsJsonAsync(result);

        return response;
    }

    private static string GetSummary(int temp)
    {
        var summary = "Mild";

        if (temp >= 32)
        {
            summary = "Hot";
        }
        else if (temp < 32 && temp >= 22)
        {
            summary = "Warm";
        }
        else if (temp <= 16 && temp > 0)
        {
            summary = "Cold";
        }
        else if (temp <= 0)
        {
            summary = "Freezing!";
        }

        return summary;
    }
}
