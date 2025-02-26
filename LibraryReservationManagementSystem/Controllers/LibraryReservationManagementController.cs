using LibraryReservationManagementSystem.Models;
using LibraryReservationManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryReservationManagementSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class LibraryReservationManagementController(ILibraryReservationManagementService service) : ControllerBase
{

    #region Context

    public class RentABookRequest
    {
        public int CustomerId { get; set; }
        public int BookId { get; set; }
    }

    #endregion

    [HttpPost("Rent", Name = "RentABook")]
    public IActionResult RentABook([FromBody] RentABookRequest request)
    {
        var result = service.RentABook(request.CustomerId, request.BookId);
        return result.IsSuccess
            ? Ok(result.Data)
            : BadRequest(result.Message);
    }

}