using Microsoft.Extensions.Options;

using MongoDB.Driver;
using Mypethere.User.Infrastructure.Repositories.DataSource.Mongo.MongoModels.UserMongoModel;
using MyPethere.User.CrossCutting;

namespace Mypethere.User.Infrastructure.Repositories.DataSource.Mongo;

public class UserMongoDatasource(IMongoDatabase database, IOptions<AppSettings> _appSettings)
    : MongoDatasource<Domain.Entities.User.User, UserMongoModel, Guid>(database, _appSettings.Value.MongoUserNameColletion), IUserMongoDatasource
{
}
