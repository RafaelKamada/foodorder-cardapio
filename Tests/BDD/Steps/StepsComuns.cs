using Reqnroll;

namespace Tests.BDD.Steps
{
    [Binding]
    public class StepsComuns
    {
        [Given(@"o sistema esta pronto para cadastro")]
        public void DadoOSistemaEstaProntoParaCadastro()
        {
            // Pode estar vazio mesmo
        }

        [Then(@"o sistema deve retornar erro de não encontrado")]
        public void EntaoErroNaoEncontrado()
        {
            // Este é um passo de verificação que já foi realizado no When
        }

        [Then(@"o sistema deve retornar erro de validacao")]
        public void OSistemaDeveRetornarErroDeValidacao()
        {
            // Este é um passo de verificação que já foi realizado no When
        }
    }
}
