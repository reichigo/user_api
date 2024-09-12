using Mypethere.User.Domain.Entities.User;

using MyPethere.User.CrossCutting.Exceptions;

using System.IdentityModel.Tokens.Jwt;

namespace Mypethere.Web.Api.ExtensionsMethods;

public static class HttpRequestExtension
{
   public static Guid GetUserId(this HttpRequest httpRequest)
    {
        var jwt = httpRequest.Headers["Authorization"].FirstOrDefault()?.Substring("Bearer ".Length).Trim();
        var tokenHe = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
        string user = tokenHe.Claims.First(c => c.Type == "sub").Value;

        return Guid.Parse(user);
    }

    public static (string, Guid) GetRefreshTokenAndUserId(this HttpRequest httpRequest)
    {
        var refreshToken = httpRequest.Headers["Authorization"].FirstOrDefault()?.Substring("Bearer ".Length).Trim();
        var userId = httpRequest.Headers["userId"].FirstOrDefault();

        if (refreshToken is null || userId is null) 
        {
            throw new BadRequestException("token or userid is missing", System.Net.HttpStatusCode.BadRequest);
        }

        return (refreshToken, Guid.Parse(userId));
    }
}
