using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using MongoDB.Bson;

using Mypethere.User.Application.DTOs.Request;
using Mypethere.User.Application.DTOs.Request.LoginRequest;
using Mypethere.User.Application.DTOs.Response;
using Mypethere.Web.Api.ExceptionsHandler;
using Mypethere.Web.Api.Utils;

using MyPethere.User.CrossCutting.Ioc;

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddAppEndpoints(Assembly.GetExecutingAssembly());
Injection.ConfigurationService(builder.Services, builder.Configuration);

// Configura o contexto de serialização
//builder.Services.AddSingleton(options =>
//{
//    return new JsonSerializerOptions
//    {
//        TypeInfoResolver = AppJsonSerializerContext.Default,
//        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//    };
//});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.TypeInfoResolver = AppJsonSerializerContext.Default;
});

builder.Services.AddExceptionHandler<RequestExceptionHandler>();

var app = builder.Build();

Injection.ConfigureApp(app);

app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext httpContext) =>
{
    var exceptionHandlerPathFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>();
    var errorDetails = new
    {
        exception = exceptionHandlerPathFeature?.Error?.Message,
        stackTrace = exceptionHandlerPathFeature?.Error?.StackTrace
    };
    return Results.Json(errorDetails, new JsonSerializerOptions { TypeInfoResolver = AppJsonSerializerContext.Default });
});

// Mapeia os endpoints
app.MapEndpoints();

app.Run();

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(object))]
[JsonSerializable(typeof(CreateUserLoginRequestDto))]
[JsonSerializable(typeof(LoginRequestDto))]
[JsonSerializable(typeof(TokenResponse))]
internal partial class AppJsonSerializerContext : JsonSerializerContext;
