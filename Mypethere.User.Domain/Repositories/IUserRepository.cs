

namespace Mypethere.User.Domain.Repositories;

public interface IUserRepository
{
    Task CreateUserWithCredentialAsync(Domain.Entities.User.User user);
}
