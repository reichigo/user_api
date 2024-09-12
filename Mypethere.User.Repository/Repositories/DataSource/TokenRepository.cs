using Mypethere.User.Domain.Repositories;
using Mypethere.User.Infrastructure.Repositories.DataSource.Redis;

namespace Mypethere.User.Infrastructure.Repositories.DataSource;

public class TokenRepository(ITokenRedisDatasource tokenRedisDatasource) : ITokenRepository
{
    public Task<string?> GetRefreshTokenById(Guid userId)
    {
        return tokenRedisDatasource.GetRefreshTokenById(userId);
    }

    public Task InsertRefreshToken(Guid userId, string refreshToken)
    {
        return tokenRedisDatasource.InsertRefreshToken(userId, refreshToken);
    }

    public Task RevokeRefreshToken(Guid userId)
    {
        return tokenRedisDatasource.RevokeRefreshToken(userId);
    }
}
