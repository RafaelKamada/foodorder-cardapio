using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Domain.Enums;

namespace Domain.Entities;

public class Produto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }

    public required string Nome { get; set; }
    public required CategoriaTipo Tipo { get; set; }
    public required decimal Preco { get; set; }
    public required string Descricao { get; set; }
    public required int TempoPreparo { get; set; }
    public List<Imagem> Imagens { get; set; } = new();
}