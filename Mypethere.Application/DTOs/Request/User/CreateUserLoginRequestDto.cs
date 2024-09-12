using Mypethere.User.Application.DTOs.Shared.Enums;

namespace Mypethere.User.Application.DTOs.Request;

public record CreateUserLoginRequestDto(
    string Name,
    string LastName,
    string  Email,
    string Password,
    string ReEnterPassword,
    GenderDto Gender,
    DateOnly BirthDate
    );
