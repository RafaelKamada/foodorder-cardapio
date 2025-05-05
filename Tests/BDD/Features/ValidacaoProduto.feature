#language: pt-br
Funcionalidade: Validar Produto
    Como um usuario do sistema
    Quero que o sistema valide corretamente os dados do produto
    Para evitar dados invalidos no sistema
    
    Cenario: Nome do produto vazio
        Dado o sistema esta pronto para cadastro
        Quando tento cadastrar um produto com nome vazio
        Entao o sistema deve retornar erro de validacao
    
    Cenario: Preco negativo
        Dado o sistema esta pronto para cadastro
        Quando tento cadastrar um produto com preco negativo
        Entao o sistema deve retornar erro de validacao
    
    Cenario: Tempo de preparo inferior ao minimo
        Dado o sistema esta pronto para cadastro
        Quando tento cadastrar um produto com tempo de preparo menor que o minimo
        Entao o sistema deve retornar erro de dominio