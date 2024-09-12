namespace Mypethere.User.Domain.Repositories;

public interface ITokenRepository
{
   Task InsertRefreshToken(Guid userId, string refreshToken);
   Task RevokeRefreshToken(Guid userId);
   Task<string?> GetRefreshTokenById(Guid userId);
}
