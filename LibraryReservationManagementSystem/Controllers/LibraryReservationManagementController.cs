using LibraryReservationManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryReservationManagementSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class LibraryReservationManagementController(LibraryReservationManagementService service) : ControllerBase
{
    
    [HttpPost("rent", Name = "RentABook")]
    public IActionResult RentABook(int customerId, int bookId)
    {
        var result = service.RentABook(customerId, bookId);
        return result.IsSuccess
            ? Ok(result.Data)
            : BadRequest(result.Message);
    }

}