using Mapster;

using Mypethere.User.Application.DTOs.Request;
using Mypethere.User.Domain.Entities.Shared;
using Mypethere.User.Domain.Entities.User;
using Mypethere.User.Infrastructure.Repositories.DataSource.Mongo.MongoModels.LoginMongoModel;
using Mypethere.User.Infrastructure.Repositories.DataSource.Mongo.MongoModels.Shared;
using Mypethere.User.Infrastructure.Repositories.DataSource.Mongo.MongoModels.UserMongoModel;

namespace Mypethere.User.Application.Mappers;

public static class Mapster
{
    public static void Configure()
    {
        TypeAdapterConfig<CreateUserLoginRequestDto, UserCredencial>
            .NewConfig()
            .MapWith(src => new UserCredencial(
                 Guid.NewGuid(),
                 src.Password,
                 src.Email,
                 new List<CredencialType>() { CredencialType.Password },
                 new Domain.Entities.User.User(
                     Guid.NewGuid(),
                     src.Name,
                     src.LastName,
                     src.Email,
                     src.BirthDate,
                     src.Gender.Adapt<Gender>(),
                     Enumerable.Empty<Domain.Entities.Shared.File>()
                     )
                 ));

        TypeAdapterConfig<UserCredencial, LoginMongoModel>
            .NewConfig()
            .MapWith(src => new LoginMongoModel(
                    Guid.NewGuid(),
                    src.User.Id,
                    src.UserName,
                    src.GetHashPassword(),
                    src.CredencialTypes.Adapt<IEnumerable<CredencialTypeMongoModel>>()
                 ));

        TypeAdapterConfig<Domain.Entities.User.User, UserMongoModel>
            .NewConfig()
            .MapWith(src => new UserMongoModel(
                    src.Id,
                    src.Name,
                    src.LastName,
                    src.Email,
                    src.BirthDate,
                    src.Gender.Adapt<GenderModelMongo>(),
                    Enumerable.Empty<string>()
                 ));

        TypeAdapterConfig<UserMongoModel, Domain.Entities.User.User>
            .NewConfig()
            .MapWith(src => new Domain.Entities.User.User(
                    src._id,
                    src.Name,
                    src.LastName,
                    src.Email,
                    src.BirthDate,
                    src.Gender.Adapt<Gender>(),
                    Enumerable.Empty<Domain.Entities.Shared.File>()
                 ));
    }
}
