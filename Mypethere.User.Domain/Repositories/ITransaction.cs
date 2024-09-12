namespace Mypethere.User.Domain.Repositories;

public interface ITransaction : IDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}
