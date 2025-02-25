using LibraryReservationManagementSystem.Models;
using LibraryReservationManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryReservationManagementSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationController(IRepositoryFactory repositoryFactory, ILogger<BookController> logger) : ControllerBase
{

    private readonly ILogger<BookController> _logger = logger;

    [HttpGet(Name = "GetReservations")]
    public IActionResult Get()
    {
        using var repository = repositoryFactory.GetRepository<Reservation>();
        var reservations = repository.GetAll();
        return reservations.IsSuccess
            ? Ok(reservations.Data)
            : NotFound(reservations.Message);
    }


    [HttpGet("{id}", Name = "GetReservation")]
    public IActionResult Get(int id)
    {
        using var repository = repositoryFactory.GetRepository<Reservation>();
        var reservation = repository.GetById(id);
        return reservation.IsSuccess
            ? Ok(reservation.Data)
            : NotFound(reservation.Message);
    }

    #region Context
    public class CreateReservationRequest
    {
        public int BookId { get; set; }
        public int CustomerId { get; set; }
        public DateTime ReservationDate { get; set; }

    }
    #endregion

    [HttpDelete("{id}", Name = "DeleteReservation")]
    public IActionResult Delete(int id)
    {
        using var repository = repositoryFactory.GetRepository<Reservation>();
        var repositoryToRemove = repository.GetById(id);

        if (!repositoryToRemove.IsSuccess || repositoryToRemove.Data == null)
            return NotFound(repositoryToRemove.Message);

        var result = repository.Delete(repositoryToRemove.Data);
        return result.IsSuccess
            ? Ok(result.Data)
            : NotFound(result.Message);
    }


}