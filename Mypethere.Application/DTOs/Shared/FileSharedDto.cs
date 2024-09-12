namespace Mypethere.User.Application.DTOs.Shared;

public record FileSharedDto(
    string Extension,
    byte[] Content,
    FileTypeSharedDto FileType
    );
