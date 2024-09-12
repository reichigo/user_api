using Mypethere.User.Infrastructure.Repositories.DataSource.Mongo.MongoModels.UserMongoModel;

namespace Mypethere.User.Infrastructure.Repositories.DataSource.Mongo;

public interface IUserMongoDatasource : IMongoDatasource<Domain.Entities.User.User, UserMongoModel, Guid>;
