using Mypethere.User.Domain.Repositories;
using System.Data;

namespace Mypethere.User.Infrastructure.Repositories.DataSource;

public class RelationalTransaction : ITransaction
{
    private readonly IDbTransaction _transaction;

    public RelationalTransaction(IDbTransaction transaction)
    {
        _transaction = transaction;
    }

    public Task CommitAsync()
    {
        _transaction.Commit();
        return Task.CompletedTask;
    }

    public Task RollbackAsync()
    {
        _transaction.Rollback();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _transaction.Dispose();
    }
}
