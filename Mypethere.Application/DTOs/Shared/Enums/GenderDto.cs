using System.Text.Json.Serialization;

namespace Mypethere.User.Application.DTOs.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GenderDto
{
    Male = 0,
    Female = 1,
}
