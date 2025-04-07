using Domain.Exceptions;

namespace Domain.Exceptions;

public class ExcecaoValidacao : ExcecaoDominio
{
    public ExcecaoValidacao(string mensagem) : base("ExcecaoValidacao", mensagem)
    {
    }
}