using MediatR;

using Mypethere.User.Application.DTOs.Request;
using Mypethere.User.Application.DTOs.Request.LoginRequest;
using Mypethere.User.Application.LoginUsecase.Create;
using Mypethere.User.Application.LoginUsecase.LoginCredencial;
using Mypethere.User.Application.LoginUsecase.RefreshToken;
using Mypethere.Web.Api.ExtensionsMethods;
using Mypethere.Web.Api.Utils;

namespace Mypethere.Web.Api.Endpoints
{
    public class LoginEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/Login/CreateCredential", async (
                CreateUserLoginRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
                   {
                       await sender.Send(new CreateLoginCredencialCommand(request));
                       return Results.Ok();

                   }).WithName("CreateLoginCrencial")
                .WithOpenApi();


            app.MapPost("/Login",
                async (
            LoginRequestDto request,
            ISender sender,
            HttpRequest httpRequest,
            CancellationToken cancellationToken) =>
            {

                var token = await sender.Send(new LoginByCredentialQuery(request.UserName, request.Password));

                return Results.Ok(token);

            }).WithName("MakeLogin")
            .WithOpenApi();


            app.MapPost("/Login/RefreshToken",
                async (
            ISender sender,
            HttpRequest httpRequest,
            CancellationToken cancellationToken) =>
                {
                    var (refreshToken, userId) = httpRequest.GetRefreshTokenAndUserId();

                    var token = await sender.Send(new RefreshTokenCommand(refreshToken, userId));

                    return Results.Ok(token);

                }).WithName("RefreshToken")
            .WithOpenApi();
        }

    }
}
