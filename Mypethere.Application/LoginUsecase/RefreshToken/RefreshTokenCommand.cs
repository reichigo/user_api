using Mypethere.User.Application.DTOs.Response;

namespace Mypethere.User.Application.LoginUsecase.RefreshToken;

public record RefreshTokenCommand(string refreshToken, Guid userId) : IRequest<TokenResponse>;
