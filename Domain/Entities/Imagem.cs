using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Imagem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }
    public required DateTime Data { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public required string ProdutoId { get; set; }
    public required string Base64Data { get; set; }
}