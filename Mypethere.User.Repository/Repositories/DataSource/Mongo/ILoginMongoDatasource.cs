using Mypethere.User.Infrastructure.Repositories.DataSource.Mongo.MongoModels.LoginMongoModel;

namespace Mypethere.User.Infrastructure.Repositories.DataSource.Mongo;

public interface ILoginMongoDatasource : IMongoDatasource<UserCredencial, LoginMongoModel, Guid>
{
    Task CreateLoginAsync(UserCredencial userCredencial);
    Task<bool> CheckLoginPassworkAsync(string userName, string hashedPasswork);
    Task<bool> IsLoginCredencialTypeAlreadyExistAsync(string userName, CredencialType credencialType);
    Task<UserCredencial> GetCredenciaByUserName(string userName);
}
