using MediatR;

using Mypethere.User.Application.DTOs.Response;

namespace Mypethere.User.Application.LoginUsecase.LoginCredencial;

public record LoginByCredentialQuery(string UserName, string Password) : IRequest<TokenResponse>;
