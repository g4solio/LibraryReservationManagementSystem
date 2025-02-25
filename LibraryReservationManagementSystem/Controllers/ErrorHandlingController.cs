using Microsoft.AspNetCore.Mvc;

namespace LibraryReservationManagementSystem.Controllers;

public class ErrorHandlingController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/error")]
    public IActionResult HandleError() =>
        Problem();
}