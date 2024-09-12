using Mypethere.User.Domain.Repositories;

namespace Mypethere.User.Infrastructure.Repositories.DataSource.Mongo;

public interface IMongoDatasource<T, REntity, TId> : IRepository<T, REntity, TId> where T : class
{
    Task AddToArrayAsync<TArrayItem, TArrayItemMongo>(TId id, string arrayFieldName, IEnumerable<TArrayItem> arrayItem);
}
