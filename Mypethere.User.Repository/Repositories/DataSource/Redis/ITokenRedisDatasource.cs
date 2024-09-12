namespace Mypethere.User.Infrastructure.Repositories.DataSource.Redis;

public interface ITokenRedisDatasource
{
    Task InsertRefreshToken(Guid userId, string refreshToken);
    Task RevokeRefreshToken(Guid userId);
    Task<string?> GetRefreshTokenById(Guid userId);
}
