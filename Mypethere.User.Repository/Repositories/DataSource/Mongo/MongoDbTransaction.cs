using Mypethere.User.Domain.Repositories;

namespace Mypethere.User.Infrastructure.Repositories.DataSource.Mongo;

public class MongoDbTransaction : ITransaction
{
    public IClientSessionHandle Session { get; }

    public MongoDbTransaction(IClientSessionHandle session)
    {
        Session = session;
    }

    public async Task CommitAsync()
    {
        await Session.CommitTransactionAsync();
    }

    public async Task RollbackAsync()
    {
        await Session.AbortTransactionAsync();
    }

    public void Dispose()
    {
        Session.Dispose();
    }
}
