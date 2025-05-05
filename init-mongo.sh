#!/bin/bash

# Aguarda o MongoDB estar pronto
echo "Aguardando MongoDB estar pronto..."
sleep 5

# Verifica se o arquivo existe
if [ -f "/docker-entrypoint-initdb.d/init.js" ]; then
    echo "Arquivo init.js encontrado, executando..."
    mongosh /docker-entrypoint-initdb.d/init.js
else
    echo "ERRO: Arquivo init.js não encontrado!"
fi