#language: pt-br
Funcionalidade: Cadastrar Produto
	Como um usuario do sistema
	Quero cadastrar um novo produto
	Para poder adiciona-lo ao cardapio

	Cenário: Adicionar produto com sucesso
		Dado o sistema esta pronto para cadastro
		Quando eu envio os dados de um novo produto
		Entao o produto e salvo no banco de dados