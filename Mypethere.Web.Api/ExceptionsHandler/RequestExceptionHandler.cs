using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Server.Kestrel.Core;

using MyPethere.User.CrossCutting.Exceptions;

using System.Net;

namespace Mypethere.Web.Api.ExceptionsHandler;

public class RequestExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BadRequestException)
        {
            return false;
        }

        var httpStatusCodse = exception switch
        {
            BadRequestException or ArgumentException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError,
        };

        if(exception is BadRequestException badRequestException)
        {
            httpContext.Response.StatusCode = (int)badRequestException.StatusCode;
        }

        await httpContext.Response.WriteAsync(exception.Message, cancellationToken);

        return true;
    }
}
