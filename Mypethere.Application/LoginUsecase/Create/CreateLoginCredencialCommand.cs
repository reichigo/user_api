using Mypethere.User.Application.DTOs.Request;

namespace Mypethere.User.Application.LoginUsecase.Create;

public record CreateLoginCredencialCommand(CreateUserLoginRequestDto CreateUserLoginRequestDto) : IRequest;
