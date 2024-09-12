using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Mypethere.Web.Api.Utils;

public class ExtractSubClaimAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Obter o token do header "Authorization"
        var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = authorizationHeader.Substring("Bearer ".Length).Trim();
        var userId = GetUserIdFromSubClaim(token);

        if (userId == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Adicionar o UserId ao contexto da requisição
        context.HttpContext.Items["UserId"] = userId;

        // Prosseguir com a execução da ação
        await next();
    }

    private string? GetUserIdFromSubClaim(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (tokenHandler.CanReadToken(token))
        {
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var subClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            return subClaim?.Value;
        }

        return null;
    }
}
