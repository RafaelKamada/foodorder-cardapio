namespace Domain.Exceptions;

public class ExcecaoDominio : Exception
{
    public string Codigo { get; }

    public ExcecaoDominio(string codigo, string mensagem) : base(mensagem)
    {
        Codigo = codigo;
    }
}