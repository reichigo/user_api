using Mapster;
using Mypethere.User.Infrastructure.Repositories.DataSource.Mongo.MongoModels.LoginMongoModel;
using Mypethere.User.Infrastructure.Repositories.DataSource.Mongo.MongoModels.UserMongoModel;
using MyPethere.User.CrossCutting.Exceptions;

namespace Mypethere.User.Infrastructure.Repositories.DataSource.Mongo;

public class LoginMongoDatasource(
    IMongoDatabase database,
    IOptions<AppSettings> _appSettings,
    IUserMongoDatasource _userMongoDatasource
    )
    : MongoDatasource<UserCredencial, LoginMongoModel, Guid>(database, _appSettings.Value.MongoLoginNameCollection),
    ILoginMongoDatasource
{
    public Task<bool> CheckLoginPassworkAsync(string userName, string hashedPasswork)
    {
        return ExistsAsync(x => x.UserName == userName && x.Password == hashedPasswork);
    }

    public Task<bool> IsLoginCredencialTypeAlreadyExistAsync(string userName, CredencialType credencialType)
    {
        var credentialTypeMongo = credencialType.Adapt<CredencialTypeMongoModel>();

        return ExistsAsync(x => x.UserName == userName && x.CredencialTypes.Contains(credentialTypeMongo));
    }

    public async Task CreateLoginAsync(UserCredencial userCredencial)
    {
        using var session = await Database.Client.StartSessionAsync();
        session.StartTransaction();

        await CreateAsync(userCredencial, new MongoDbTransaction(session));

        await _userMongoDatasource.CreateAsync(userCredencial.User, new MongoDbTransaction(session));

        session.CommitTransaction();
    }

    public async Task<UserCredencial> GetCredenciaByUserName(string userName)
    {
        var colletionLoginMongoModel = database.GetCollection<LoginMongoModel>(_appSettings.Value.MongoLoginNameCollection);
        var colletionUser = database.GetCollection<UserMongoModel>(_appSettings.Value.MongoUserNameColletion);

        var loginMongoModel = await colletionLoginMongoModel.Find(Builders<LoginMongoModel>.Filter.Eq("UserName", userName)).FirstOrDefaultAsync();

        if (loginMongoModel == null)
        {
            throw new BadRequestException("Credential not Found", System.Net.HttpStatusCode.BadRequest);
        }

        var user = await colletionUser.Find(Builders<UserMongoModel>.Filter.Eq("_id", loginMongoModel.UserId)).FirstOrDefaultAsync();

        return new UserCredencial(
            loginMongoModel._id,
            loginMongoModel.Password,
            loginMongoModel.UserName,
            loginMongoModel.CredencialTypes.Adapt<IEnumerable<CredencialType>>(),
            user.Adapt<Domain.Entities.User.User>()
            );
    }
}
