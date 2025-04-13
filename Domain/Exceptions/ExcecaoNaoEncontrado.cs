namespace Domain.Exceptions
{
    public class ExcecaoNaoEncontrado : ExcecaoDominio
    {
        public ExcecaoNaoEncontrado(string mensagem) : base("ExcecaoNaoEncontrado", mensagem)
        {
        }
    }
}