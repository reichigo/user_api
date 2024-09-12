using Mapster;
using Mypethere.User.Domain.Repositories;
using Mypethere.User.Infrastructure.Repositories.DataSource.Mongo;
using Mypethere.User.Infrastructure.Repositories.DataSource.Mongo.MongoModels.LoginMongoModel;

namespace Mypethere.User.Infrastructure.Repositories.DataSource;

public class LoginRepository(
    IMongoDatabase _database,
    IOptions<AppSettings> _appSettings,
    ILoginMongoDatasource _loginMongoDatasource
    )
    : MongoDatasource<Domain.Entities.User.User, LoginMongoModel, Guid>(_database, _appSettings.Value.MongoLoginNameCollection)
    , ILoginRepository
{
    public Task<bool> IsLoginCredencialTypeAlreadyExistAsync(string userName, CredencialType credencialType)
    {
        return _loginMongoDatasource.IsLoginCredencialTypeAlreadyExistAsync(userName, credencialType);
    }

    public Task<bool> CheckLoginPasswork(string userName, string hashedPasswork)
    {
        return _loginMongoDatasource.CheckLoginPassworkAsync(userName, hashedPasswork);
    }

    public async Task CreateCredentialAsync(UserCredencial userCredencial)
    {
        await _loginMongoDatasource.CreateLoginAsync(userCredencial);
    }

    public Task<UserCredencial> GetCredenciaByUserName(string userName)
    {
        return _loginMongoDatasource.GetCredenciaByUserName(userName);
    }
}
