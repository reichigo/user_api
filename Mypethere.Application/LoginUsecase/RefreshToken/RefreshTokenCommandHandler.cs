
using Mypethere.User.Application.DTOs.Response;
using Mypethere.User.Application.Services;

namespace Mypethere.User.Application.LoginUsecase.RefreshToken;

public record RefreshTokenCommandHandler(ITokenService tokenService) : IRequestHandler<RefreshTokenCommand, TokenResponse>
{
    public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var (accessToken, refreshToken) = await tokenService.RefreshTokens(request.userId, request.refreshToken);

        return new TokenResponse(accessToken, refreshToken);   
    }
}
