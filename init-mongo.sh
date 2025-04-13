#!/bin/bash

# Aguarda o MongoDB estar pronto
echo "Aguardando MongoDB estar pronto..."
sleep 5

# Cria as collections com �ndices
echo "Iniciando cria��o das collections..."

# Collection de Produtos
mongo --eval 'db.createCollection("produtos")'
mongo --eval 'db.produtos.createIndex({ "Nome": 1 })'
mongo --eval 'db.produtos.createIndex({ "Tipo": 1 })'
mongo --eval 'db.produtos.createIndex({ "IdSequencial": 1 })'

# Collection de Imagens
mongo --eval 'db.createCollection("imagens")'
mongo --eval 'db.imagens.createIndex({ "ProdutoId": 1 })'

mongo --eval 'db.createCollection("sequences");'
mongo FoodOrder_Cardapio --eval '
db.sequences.insertOne({
    _id: "Produto",
    value: 1
});'

mongo FoodOrder_Cardapio --eval '
db.produtos.insertMany([
    {
        idSequencial: 1,
        nome: "X-Burger",
        preco: 15.99,
        descricao: "Hambúrguer artesanal com queijo cheddar",
        tipo: "Lanche",
        tempoPreparo: 15,
        imagens: [
            {
                data: "data:image/jpeg;base64,...",
                produtoId: ObjectId()
            }
        ]
    },
    {
        idSequencial: 2,
        nome: "Batata Frita",
        preco: 8.99,
        descricao: "Batata frita crocante",
        tipo: "Acompanhamento",
        tempoPreparo: 10,
        imagens: [
            {
                data: "data:image/jpeg;base64,...",
                produtoId: ObjectId()
            }
        ]
    },
    {
        idSequencial: 3,
        nome: "Refrigerante",
        preco: 6.99,
        descricao: "Refrigerante 600ml",
        tipo: "Bebida",
        tempoPreparo: 1,
        imagens: [
            {
                data: "data:image/jpeg;base64,...",
                produtoId: ObjectId()
            }
        ]
    }
]);'

echo "Collections criadas com sucesso!"

# executar o comando abaixo: 
# chmod +x init-mongo.sh
