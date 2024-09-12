namespace Mypethere.User.Domain.Entities.User;

public record User(
    Guid Id,
    string Name,
    string LastName,
    string Email,
    DateOnly BirthDate,
    Gender Gender,
    IEnumerable<Shared.File> Files
    );
