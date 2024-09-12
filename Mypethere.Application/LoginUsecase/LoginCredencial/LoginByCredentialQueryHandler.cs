using MediatR;

using Mypethere.User.Application.DTOs.Response;
using Mypethere.User.Application.Services;
using Mypethere.User.Domain.Entities.Credencial;
using Mypethere.User.Domain.Repositories;

using MyPethere.User.CrossCutting;
using MyPethere.User.CrossCutting.Exceptions;

namespace Mypethere.User.Application.LoginUsecase.LoginCredencial;

public class LoginByCredentialQueryHandler(
    ILoginRepository _loginRepository,
    ITokenService tokenService,
    ITokenRepository tokenRepository
    ) : IRequestHandler<LoginByCredentialQuery, TokenResponse>
{
    public async Task<TokenResponse> Handle(LoginByCredentialQuery request, CancellationToken cancellationToken)
    {
        var credential = await _loginRepository.GetCredenciaByUserName(request.UserName);

        if (!credential.IsPasswordHashValid(request.Password))
        {
            throw new BadRequestException("Password is not match", System.Net.HttpStatusCode.BadRequest);
        }

        var (accessToken, refreshToken) = tokenService.CreateTokens(credential.User.Id);

        await tokenRepository.RevokeRefreshToken(credential.User.Id);

        await tokenRepository.InsertRefreshToken(credential.User.Id, refreshToken);

        return new TokenResponse(accessToken, refreshToken);
    }
}
