using StackExchange.Redis;

namespace Mypethere.User.Infrastructure.Repositories.DataSource.Redis;

public class TokenRedisDatasource(IConnectionMultiplexer redis) : ITokenRedisDatasource
{
    public async Task<string?> GetRefreshTokenById(Guid userId)
    {
        var db = redis.GetDatabase();

        var key = $"token-{userId}";

        var refreshToken = await db.StringGetAsync(key);

        return refreshToken;
    }

    public Task InsertRefreshToken(Guid userId, string refreshToken)
    {
        var db = redis.GetDatabase();

        var key = $"token-{userId}";

        return db.StringSetAsync(key, refreshToken);
    }

    public Task RevokeRefreshToken(Guid userId)
    {
        var db = redis.GetDatabase();

        var key = $"token-{userId}";

        return db.KeyDeleteAsync(key);
    }
}
