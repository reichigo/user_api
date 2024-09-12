using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using MongoDB.Driver;

using Mypethere.User.Application.Services;
using Mypethere.User.Domain.Repositories;
using Mypethere.User.Infrastructure.Repositories.DataSource;
using Mypethere.User.Infrastructure.Repositories.DataSource.Mongo;
using Mypethere.User.Infrastructure.Repositories.DataSource.Redis;
using StackExchange.Redis;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;

namespace MyPethere.User.CrossCutting.Ioc;

public static class Injection
{
    public static void ConfigurationService(IServiceCollection services, ConfigurationManager configuration)
    {
        // Adicione o appsettings.json à configuração
        configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        // Configure a injeção de dependência para a classe de configuração
        services.Configure<AppSettings>(configuration.GetSection("Values"));

        Mypethere.User.Application.Mappers.Mapster.Configure();

        services.AddTransient<ISerializerDataContractResolver>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<JsonOptions>>().Value?.SerializerOptions
                ?? new JsonSerializerOptions(JsonSerializerDefaults.Web);

            return new JsonSerializerDataContractResolver(opts);
        });

        // Configurações de serviços
        var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var applicationAssembly = Directory.GetFiles(path, "MyPethere.User.Application.dll").Select(AssemblyLoadContext.Default.LoadFromAssemblyPath).FirstOrDefault();

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(applicationAssembly);
        });

        // Validation for FluentValidation
        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SupportNonNullableReferenceTypes();
            c.UseAllOfToExtendReferenceSchemas();
            c.SchemaFilter<EnumSchemaFilter>();
        });

        // Adiciona as interfaces do sistema
        services.AddSingleton<IMongoClient>(s =>
        {
            var connectionString = configuration["Values:MongoConnectionString"];
            return new MongoClient(connectionString);
        });

        services.AddSingleton(s =>
        {
            var client = s.GetRequiredService<IMongoClient>();
            var databaseName = configuration["Values:MongoUserNameDataBase"];
            return client.GetDatabase(databaseName);
        });

        services.AddSingleton<IConnectionMultiplexer>(s =>
        {
            var connectionString = configuration["Values:RedisConnectionString"];
            return ConnectionMultiplexer.Connect(connectionString);
        });

        // Registrar repositórios
        services.AddSingleton<ILoginMongoDatasource, LoginMongoDatasource>();
        services.AddSingleton<ILoginRepository, LoginRepository>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IUserMongoDatasource, UserMongoDatasource>();
        services.AddSingleton<ITokenRepository, TokenRepository>();
        services.AddSingleton<ITokenRedisDatasource, TokenRedisDatasource>();

        // Registrar serviços
        services.AddSingleton<ITokenService, TokenService>();

        // Registrar RegexInlineRouteConstraint
        services.Configure<RouteOptions>(options =>
        {
            options.ConstraintMap["regex"] = typeof(RegexInlineRouteConstraint);
        });
    }

    public static void ConfigureApp(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear();
            foreach (var name in Enum.GetNames(context.Type))
            {
                schema.Enum.Add(new OpenApiString(name));
            }
        }

        if (context.Type == typeof(byte[]))
        {
            schema.Type = "string";
            schema.Format = "byte";
            schema.Example = new OpenApiString("98 66 9c fb b7 0e eb e1 d0 5e");
        }

        if (context.Type == typeof(int))
        {
            schema.Example = new OpenApiInteger(10);
        }

        if (context.Type == typeof(double))
        {
            schema.Example = new OpenApiDouble(10.0);
        }
    }
}
