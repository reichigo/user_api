
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Mypethere.User.Infrastructure.Repositories.DataSource.Mongo.MongoModels.Shared;

namespace Mypethere.User.Infrastructure.Repositories.DataSource.Mongo.MongoModels.UserMongoModel;

public record UserMongoModel(
    [property: BsonId, BsonRepresentation(BsonType.String)] Guid _id,
    string Name,
    string LastName,
    string Email,
    DateOnly BirthDate,
    [property: BsonRepresentation(BsonType.String)] GenderModelMongo Gender,
    IEnumerable<string> Files
    );
