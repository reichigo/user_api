using MediatR;

using Mypethere.User.Application.DTOs.Request;

namespace Mypethere.User.Application.Users.Create;

public record CreateUserCommand(CreateUserLoginRequestDto createUserLoginRequestDto) : IRequest;
