using System.Net;

namespace MyPethere.User.CrossCutting.Exceptions;

public class BadRequestException(string Message, HttpStatusCode statusCode) : Exception(Message) 
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}
