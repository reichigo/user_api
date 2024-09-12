namespace Mypethere.User.Application.Services;
public interface ITokenService
{
    (string AccessToken, string RefreshToken) CreateTokens(Guid userId);
    Task<(string AccessToken, string RefreshToken)> RefreshTokens(Guid userId, string refreshToken);
    Task RevokeRefreshToken(Guid userId);
}


