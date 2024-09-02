using ChallengePoint.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ChallengePoint.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    public class ThrowController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error()
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionHandlerFeature?.Error;

            if (exception == null)
            {
                return Problem(detail: "An unknown error occurred.", statusCode: 500);
            }

            return exception switch
            {
                NotFoundException => NotFound(new { message = "The requested resource was not found." }),
                ApiException apiException => StatusCode(apiException.StatusCode, apiException),
                _ => StatusCode(500, new { message = "An unexpected error occurred. Please try again later." }),
            };
        }
    }
}