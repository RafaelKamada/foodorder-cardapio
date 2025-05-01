#language: pt-br
Funcionalidade: Listagem de Produtos
  Como um usuário do sistema
  Quero poder visualizar todos os produtos cadastrados
  Para gerenciar o cardápio

  Cenário: Listar todos os produtos
    Dado existem produtos cadastrados no sistema
    Quando solicito a listagem de produtos
    Então o sistema deve retornar a lista completa de produtos

  Cenário: Listar produtos por categoria
    Dado existem produtos cadastrados no sistema
    Quando solicito a listagem de produtos por categoria
    Então o sistema deve retornar apenas os produtos daquela categoria