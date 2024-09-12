namespace Mypethere.User.Domain.Repositories;

public interface ILoginRepository
{
    Task CreateCredentialAsync(UserCredencial userCredencial);
    Task<bool> CheckLoginPasswork(string userName, string hashedPasswork);
    Task<UserCredencial> GetCredenciaByUserName(string userName);
    public Task<bool> IsLoginCredencialTypeAlreadyExistAsync(string userName, CredencialType credencialType);
}
