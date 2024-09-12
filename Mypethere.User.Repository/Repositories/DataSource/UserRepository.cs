using Mypethere.User.Domain.Repositories;
using Mypethere.User.Infrastructure.Repositories.DataSource.Mongo;
using Mypethere.User.Infrastructure.Repositories.DataSource.Mongo.MongoModels.UserMongoModel;

namespace Mypethere.User.Infrastructure.Repositories.DataSource;

public class UserRepository(IMongoDatabase database, IOptions<AppSettings> _appSettings)
    : MongoDatasource<Domain.Entities.User.User, UserMongoModel, Guid>(database, _appSettings.Value.MongoUserNameColletion)
    , IUserRepository
{
    public Task CreateUserWithCredentialAsync(Domain.Entities.User.User user)
    {
        throw new NotImplementedException();
    }
}
