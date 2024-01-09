using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ProductManagement.API.Controllers;

/// <summary>
/// Global error handler - https://learn.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-8.0#exception-handler
/// In this controller we handle all the errors and we can make use of 'Problem Details for HTTP APIs' as defined in RFC9457 https://datatracker.ietf.org/doc/html/rfc9457
/// *Other frequently used approaches are 
///     1. Error Handling Middleware or 
///     2. Exception Filter Attribute
/// *TODO (Extendability): 
///     1. The approach used here can take advantage of a custom implementation 
///         of ProblemDetailsFactory - https://learn.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-8.0#implement-problemdetailsfactory
///     2. The errors can be logged in application insights or inject a service (i.e IExceptionServcie) 
///         and send them to application layer to decide what to do for example to persist them in database or integrate them with other service.
///     3. Expose more endpoints so the Admin can access and manage the errors.
/// </summary>
public class ErrorsController : ControllerBase
{
    private readonly ILogger<ErrorsController> _logger;

    public ErrorsController(ILogger<ErrorsController> logger)
    {
        _logger = logger;
    }

    [Route("/error")]
    //Used to exclude from swagger.
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception == null)
            return Problem(statusCode: (int)HttpStatusCode.BadRequest);

        //TODO: Send exceptions to the application layer to persist them or to integrate them to other service.
        // For example inject IExceptionHandlerService and then call IExceptionHandlerService.Create(exception)

        _logger.LogError(exception.Message, exception.InnerException);

        return Problem(title: exception.Message);
    }

    //TODO: If exceptions are persisted in the database and are not handled by a centralized service,
    //  then expose more endpoints here so Admin and developers can interact.
}
