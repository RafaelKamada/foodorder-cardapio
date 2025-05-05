#language: pt-br
Funcionalidade: Atualização de Produto
    Como um usuário do sistema
    Quero poder atualizar as informações de um produto
    Para manter o cardápio atualizado

    Cenário: Atualizar produto existente
        Dado um produto ja cadastrado no sistema
        Quando atualizo as informações do produto
        Então o produto deve ser atualizado com sucesso

    Cenário: Atualizar produto inexistente
        Dado o sistema está pronto para atualização
        Quando tento atualizar um produto inexistente
        Então o sistema deve retornar erro de não encontrado