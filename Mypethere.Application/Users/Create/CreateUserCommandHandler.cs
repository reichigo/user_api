using FluentValidation;

using Konscious.Security.Cryptography;

using Mapster;

using MediatR;

using Mypethere.User.Application.DTOs.Request;
using Mypethere.User.Domain.Entities.User;
using Mypethere.User.Domain.Repositories;

using System.Security.Cryptography;
using System.Text;

namespace Mypethere.User.Application.Users.Create;

public class CreateUserCommandHandler(
    IValidator<CreateUserLoginRequestDto> _validator,
    ILoginRepository _loginRepository)
     : IRequestHandler<CreateUserCommand>
{
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request.createUserLoginRequestDto);
        var userCredencial = request.createUserLoginRequestDto.Adapt<UserCredencial>();

        await _loginRepository.CreateCredentialAsync(userCredencial);

    }

    private string HashPassword(string password)
    {
        var salt = GenerateSalt();
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = 8, // número de threads a serem usados
            MemorySize = 65536, // 64 MB
            Iterations = 4 // número de iterações
        };

        return Convert.ToBase64String(argon2.GetBytes(16)) + ":" + Convert.ToBase64String(salt);
    }

    private byte[] GenerateSalt()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            var salt = new byte[16];
            rng.GetBytes(salt);
            return salt;
        }
    }

    //public bool VerifyPassword(string password, string hashedPassword)
    //{
    //    var parts = hashedPassword.Split(':');
    //    var hash = Convert.FromBase64String(parts[0]);
    //    var salt = Convert.FromBase64String(parts[1]);

    //    var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
    //    {
    //        Salt = salt,
    //        DegreeOfParallelism = 8, // número de threads a serem usados
    //        MemorySize = 65536, // 64 MB
    //        Iterations = 4 // número de iterações
    //    };

    //    var computedHash = argon2.GetBytes(16);
    //    return hash.SequenceEqual(computedHash);
    //}
}
