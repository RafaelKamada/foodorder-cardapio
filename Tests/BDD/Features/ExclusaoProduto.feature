#language: pt-br
Funcionalidade: Exclusão de Produto
  Como um usuário do sistema
  Quero poder remover um produto do cardápio
  Para manter o catálogo atualizado

  Cenário: Excluir produto existente
    Dado um produto já cadastrado no sistema
    Quando solicito a exclusão do produto
    Então o produto deve ser removido do sistema

  Cenário: Excluir produto inexistente
    Dado o sistema está pronto para exclusão
    Quando tento excluir um produto inexistente
    Então o sistema deve retornar erro de não encontrado