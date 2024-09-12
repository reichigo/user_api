using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mypethere.User.Infrastructure.Repositories.DataSource.Mongo.MongoModels.LoginMongoModel;

public record LoginMongoModel(
    [property: BsonId, BsonRepresentation(BsonType.String)] Guid _id,
    [property: BsonRepresentation(BsonType.String)] Guid UserId,
    string? UserName,
    string? Password,
    [property: BsonRepresentation(BsonType.String)] IEnumerable<CredencialTypeMongoModel> CredencialTypes
);