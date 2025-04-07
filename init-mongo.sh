#!/bin/bash

# Aguarda o MongoDB estar pronto
echo "Aguardando MongoDB estar pronto..."
sleep 5

# Cria as collections com índices
echo "Iniciando criação das collections..."

# Collection de Produtos
mongo --eval 'db.createCollection("produtos")'
mongo --eval 'db.produtos.createIndex({ "Id": 1 }, { unique: true })'
mongo --eval 'db.produtos.createIndex({ "Nome": 1 })'
mongo --eval 'db.produtos.createIndex({ "Tipo": 1 })'

# Collection de Categorias
mongo --eval 'db.createCollection("categorias")'
mongo --eval 'db.categorias.createIndex({ "Id": 1 }, { unique: true })'
mongo --eval 'db.categorias.createIndex({ "Nome": 1 })'

# Collection de Imagens
mongo --eval 'db.createCollection("imagens")'
mongo --eval 'db.imagens.createIndex({ "Id": 1 }, { unique: true })'
mongo --eval 'db.imagens.createIndex({ "ProdutoId": 1 })'

echo "Collections criadas com sucesso!"

# executar o comando abaixo: 
# chmod +x init-mongo.sh
