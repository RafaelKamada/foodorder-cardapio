using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Domain.Exceptions;

namespace Domain.Entities;

public class Produto
{
    private const int TEMPO_PREPARO_MINIMO = 5;
    private const string CODIGO_TEMPO_PREPARO_INVALIDO = "PRODUTO_TEMPO_PREPARO_INVALIDO";

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int IdSequencial { get; set; }
    public string Nome { get; set; }
    public string Tipo { get; set; }
    public decimal Preco { get; set; }
    public string Descricao { get; set; }
    public int TempoPreparo { get; set; }
    public List<Imagem> Imagens { get; set; } = new();

    public Produto()
    { 
    }

    public Produto(string id, string nome, string tipo, decimal preco, string descricao, int tempoPreparo, List<Imagem> imagens)
    {
        Id = id;
        Nome = nome ?? throw new ExcecaoValidacao("O nome do produto é obrigatório");
        Tipo = tipo ?? throw new ExcecaoValidacao("O tipo do produto é obrigatório");
        Preco = preco < 0 ? throw new ExcecaoValidacao("preço deve ser superior a zero") : preco;
        TempoPreparo = tempoPreparo;
        ValidarTempoPreparo(tempoPreparo);
        Descricao = descricao;
        Imagens = imagens;
    }

    private void ValidarTempoPreparo(int tempoPreparo)
    {
        if (tempoPreparo < TEMPO_PREPARO_MINIMO)
        {
            throw new ExcecaoDominio($"O tempo de preparo deve ser no mínimo {TEMPO_PREPARO_MINIMO} minutos", CODIGO_TEMPO_PREPARO_INVALIDO);
        }
        TempoPreparo = tempoPreparo;
    }
}