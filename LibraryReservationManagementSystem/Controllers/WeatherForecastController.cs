using LibraryReservationManagementSystem.DbContexts;
using LibraryReservationManagementSystem.Models;
using LibraryReservationManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryReservationManagementSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly BookRepository _repository;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, BookRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _repository.Add(new Book()
        {
            Author = "",
            ISBN = "",
            Title = "",
            Status = Book.StatusEnum.Available
        });
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
