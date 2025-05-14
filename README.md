# FoodOrderCardapio

Microserviço responsável pelo módulo de cardápios da arquitetura de microserviços do sistema FoodOrder, desenvolvido em .NET e MongoDB.

## Cobertura de Testes

Acesse o relatório de cobertura gerado [Cobertura](https://rafaelkamada.github.io/foodorder-cardapio/).

![Relatório de Cobertura](https://raw.githubusercontent.com/RafaelKamada/foodorder-cardapio/fase_4_b/docs/print_cobertura.png)
 
## Uso

### Rodando localmente

Para rodar o serviço localmente, você precisa ter Docker e .NET 8 instalados.

Para construir e rodar o serviço, utilize o comando:

```bash
docker-compose up --build -d
```

Esse comando irá:

* Criar a rede Docker para comunicação entre os serviços.
* Subir o banco de dados MongoDB.
* Iniciar o serviço `foodorder-cardapio`.

Para parar e remover os containers, use:

```bash
docker-compose down
```

### Executando os Testes

Para executar os testes, você pode usar o seguinte comando após subir os containers:

```bash
docker-compose exec api dotnet test
```

Certifique-se de que o serviço `api` esteja definido corretamente no seu arquivo `docker-compose.yml`.

### Endpoints Disponíveis

| Método | Endpoint                                | Descrição                                     |
| ------ | --------------------------------------- | --------------------------------------------- |
| GET    | /Produtos/ObterTodos                    | Retorna todos os produtos.                    |
| GET    | /Produtos/{id}                          | Retorna um produto específico pelo seu ID.    |
| PUT    | /Produtos/{id}                          | Atualiza um produto existente pelo seu ID.    |
| DELETE | /Produtos/{id}                          | Remove um produto pelo seu ID.                |
| GET    | /Produtos/ObterPorIds                   | Retorna produtos pelos seus IDs.              |
| GET    | /Produtos/ObterCategorias               | Retorna todas as categorias disponíveis.      |
| GET    | /Produtos/ObterPorCategoria/{categoria} | Retorna produtos de uma categoria específica. |
| POST   | /Produtos                               | Cria um novo produto.                         |

### Swagger

A documentação da API está disponível no Swagger, acessível em:

```
http://localhost:5000/swagger
```

Certifique-se de que o serviço esteja rodando antes de acessar.

## Variáveis de Ambiente

As seguintes variáveis de ambiente são esperadas para o correto funcionamento do serviço:

* `DB_CONNECTION_STRING`: String de conexão com o banco de dados MongoDB.
* `ASPNETCORE_ENVIRONMENT`: Ambiente de execução (`Development`, `Staging` ou `Production`).

Para fins de desenvolvimento, você pode usar as variáveis padrão do Docker Compose incluídas no arquivo `.env`.

## Contribuições

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou enviar pull requests.
