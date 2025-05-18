# :hamburger: Food Order Cardápio
![FoodOrder](foodorder.png?raw=true "FoodOrder")

## :pencil: Descrição do Projeto
<p align="left">Este projeto tem como objetivo concluir as  entregas do Tech Challenge do curso de Software Architecture da Pós Graduação da FIAP 2024/2025.
Este repositório constrói um serviço que faz parte de uma arquitetura de microsserviços.</p>

## 📊 Code Coverage
[![Coverage Status](https://coveralls.io/repos/github/RafaelKamada/foodorder-cardapio/badge.svg?branch=main)](https://coveralls.io/github/RafaelKamada/foodorder-cardapio?branch=main)

Acesse o relatório de cobertura gerado [Cobertura](https://rafaelkamada.github.io/foodorder-cardapio/).

![Relatório de Cobertura](https://raw.githubusercontent.com/RafaelKamada/foodorder-cardapio/fase_4_b/docs/print_cobertura.png)
 

## 🏗️ Arquitetura de Microsserviços
![Arquitetura](arquitetura.png?raw=true "Arquitetura")

### :computer: Tecnologias Utilizadas
- Linguagem escolhida: .NET
- Banco de Dados: MongoDB

### :hammer: Detalhes desse serviço
Microserviço responsável pelo módulo de cardápios da arquitetura de microserviços do sistema FoodOrder, desenvolvido em .NET e MongoDB.


### :hammer_and_wrench: Execução do projeto
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


### 🗄️ Outros repos do microserviço dessa arquitetura
- [Food Order Produção](https://github.com/diegogl12/food-order-producao)
- [Food Order Pagamento](https://github.com/diegogl12/food-order-pagamento)
- [Food Order Cardápio](https://github.com/RafaelKamada/foodorder-cardapio)
- [Food Order Pedidos](https://github.com/vilacalima/food-order-pedidos)
- [Food Order Usuários](https://github.com/RafaelKamada/FoodOrder)

### :page_with_curl: Documentações
- [Miro (todo planejamento do projeto)](https://miro.com/app/board/uXjVKhyEAME=/)


### :busts_in_silhouette: Autores
| [<img loading="lazy" src="https://avatars.githubusercontent.com/u/96452759?v=4" width=115><br><sub>Robson Vilaça - RM358345</sub>](https://github.com/vilacalima) |  [<img loading="lazy" src="https://avatars.githubusercontent.com/u/16946021?v=4" width=115><br><sub>Diego Gomes - RM358549</sub>](https://github.com/diegogl12) |  [<img loading="lazy" src="https://avatars.githubusercontent.com/u/8690168?v=4" width=115><br><sub>Nathalia Freire - RM359533</sub>](https://github.com/nathaliaifurita) |  [<img loading="lazy" src="https://avatars.githubusercontent.com/u/43392619?v=4" width=115><br><sub>Rafael Kamada - RM359345</sub>](https://github.com/RafaelKamada) |
| :---: | :---: | :---: | :---: |
