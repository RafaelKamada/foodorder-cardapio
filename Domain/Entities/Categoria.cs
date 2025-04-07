using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Domain.Enums;

namespace Domain.Entities;

public class Categoria
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }

    public required CategoriaTipo Tipo { get; set; }

    public string Nome 
    {
        get => Tipo.ToString();
        set => Tipo = (CategoriaTipo)Enum.Parse(typeof(CategoriaTipo), value);
    }
}