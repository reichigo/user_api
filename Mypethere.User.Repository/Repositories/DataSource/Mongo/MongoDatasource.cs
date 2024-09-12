using Mapster;

using MongoDB.Driver;
using Mypethere.User.Domain.Repositories;
using System.Linq.Expressions;

namespace Mypethere.User.Infrastructure.Repositories.DataSource.Mongo;

public abstract class MongoDatasource<T, REntity, TId> : IMongoDatasource<T, REntity, TId> where T : class
{

    public IMongoCollection<REntity> Collection { get; }
    public IMongoDatabase Database { get; }

    public MongoDatasource(IMongoDatabase database, string collectionName)
    {
        Collection = database.GetCollection<REntity>(collectionName);
        Database = database;
    }

    public Task AddToArrayAsync<TArrayItem, TArrayItemMongo>(TId id, string arrayFieldName, IEnumerable<TArrayItem> arrayItems)
    {
        var mongoModel = arrayItems.Select(x => x.Adapt<TArrayItemMongo>());
        var update = Builders<REntity>.Update.PushEach(arrayFieldName, mongoModel);
        return Collection.UpdateOneAsync(Builders<REntity>.Filter.Eq("_id", id), update);
    }

    public Task CreateAsync(T entity)
    {
        var mongoModel = entity.Adapt<REntity>();
        return Collection.InsertOneAsync(mongoModel);
    }

    public async Task CreateAsync(T entity, ITransaction transaction)
    {
        var session = (transaction as MongoDbTransaction).Session;
        var mongoModel = entity.Adapt<REntity>();

        await Collection.InsertOneAsync(session, mongoModel);
    }


    public Task DeleteAsync(TId id)
    {
        return Collection.DeleteOneAsync(Builders<REntity>.Filter.Eq("_id", id));
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var entities = await Collection.Find(entity => true).ToListAsync();
        return entities.Adapt<IEnumerable<T>>();
    }

    public Task<IEnumerable<T>> GetAllAsync(System.Data.IDbTransaction transaction)
    {
        throw new NotImplementedException();
    }

    public async Task<T> GetByIdAsync(TId id)
    {
        var entity = await Collection.Find(Builders<REntity>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
        return entity.Adapt<T>();
    }

    public async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<REntity, bool>> filter)
    {
        var entities = await Collection.Find(filter).ToListAsync();
        return entities.Adapt<IEnumerable<T>>();
    }

    public async Task<T> GetSingleByConditionAsync(Expression<Func<REntity, bool>> filter)
    {
        // Usa Find e FirstOrDefaultAsync para buscar o primeiro documento que corresponde ao filtro
        var entity = await Collection.Find(filter).FirstOrDefaultAsync();

        // Adapta a entidade encontrada para o tipo T, ou retorna o valor padrão se a entidade não for encontrada
        return entity != null ? entity.Adapt<T>() : default;
    }

    public async Task<IEnumerable<T>> GetPaginated(int pageNumber, int pageSize, Expression<Func<REntity, bool>> filter)
    {
        if (pageNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");
        }

        if (pageSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");
        }

        var filteredQuery = Collection.Find(filter)
                                       .Skip((pageNumber - 1) * pageSize)
                                       .Limit(pageSize);

        var paginatedEntities = await filteredQuery.ToListAsync();

        return paginatedEntities.Adapt<IEnumerable<T>>();
    }

    public Task UpdateAsync(TId id, T entity)
    {
        var mongoModel = entity.Adapt<REntity>();
        var idProperty = typeof(REntity).GetProperty("_id")?.GetValue(mongoModel, null)?.ToString();
        if (idProperty != null)
        {
            return Collection.ReplaceOneAsync(Builders<REntity>.Filter.Eq("_id", idProperty), mongoModel);
        }

        return Task.CompletedTask;
    }

    public Task UpdateAsync(TId id, T entity, ITransaction transaction)
    {
        var session = (transaction as MongoDbTransaction).Session;
        var mongoModel = entity.Adapt<REntity>();
        var idProperty = typeof(REntity).GetProperty("_id")?.GetValue(mongoModel, null)?.ToString();
        if (idProperty != null)
        {
            return Collection.ReplaceOneAsync(session, Builders<REntity>.Filter.Eq("_id", idProperty), mongoModel);
        }

        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Expression<Func<REntity, bool>> filter)
    {
        var entity = await Collection.Find(filter).Limit(1).FirstOrDefaultAsync();
        return entity != null;
    }
}