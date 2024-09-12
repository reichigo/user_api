using Mapster;

using MediatR;

using Mypethere.User.Domain.Entities.User;
using Mypethere.User.Domain.Repositories;

using MyPethere.User.CrossCutting.Exceptions;

using System.Net;

namespace Mypethere.User.Application.LoginUsecase.Create;

internal class CreateLoginCredencialCommandHandler(ILoginRepository loginRepository) : IRequestHandler<CreateLoginCredencialCommand>
{
    public async Task Handle(CreateLoginCredencialCommand request, CancellationToken cancellationToken)
    {
        var userCredential = request.CreateUserLoginRequestDto.Adapt<UserCredencial>();

        if (await loginRepository.IsLoginCredencialTypeAlreadyExistAsync(userCredential.UserName, userCredential.CredencialTypes.FirstOrDefault()))
        {
            throw new BadRequestException("Login already exists", HttpStatusCode.UnprocessableEntity);
        }

        await loginRepository.CreateCredentialAsync(userCredential);
    }
}
