db = db.getSiblingDB("FoodOrder_Cardapio");

// Verifica se a coleção existe
function collectionExists(name) {
    return db.getCollectionNames().includes(name);
}

// Cria coleções se não existirem
if (!collectionExists("produtos")) {
    db.createCollection("produtos");
}
if (!collectionExists("imagens")) {
    db.createCollection("imagens");
}
if (!collectionExists("sequences")) {
    db.createCollection("sequences");
}

// Cria índices se não existirem
function indexExists(collection, indexName) {
    const indexes = db[collection].getIndexes();
    return indexes.some(index => index.name === indexName);
}

if (!indexExists("produtos", "nome_1")) {
    db.produtos.createIndex({ nome: 1 });
}
if (!indexExists("produtos", "tipo_1")) {
    db.produtos.createIndex({ tipo: 1 });
}
if (!indexExists("imagens", "produtoId_1")) {
    db.imagens.createIndex({ produtoId: 1 });
}

// Garante que a sequência exista e tenha nextId = 4
const sequence = db.sequences.findOne({ _id: "produtos" });
if (sequence) {
    // Se já existe, atualiza o nextId
    db.sequences.updateOne(
        { _id: "produtos" },
        { $set: { nextId: 4 } }
    );
} else {
    // Se não existe, insere
    db.sequences.insertOne({ _id: "produtos", nextId: 4 });
}

// Insere produtos se não existirem
if (db.produtos.countDocuments() === 0) {
    db.produtos.insertMany([
        {
            IdSequencial: 1,
            Nome: "X-Burger",
            Preco: 15.99,
            Descricao: "Hamburguer artesanal com queijo cheddar",
            Tipo: "Lanche",
            TempoPreparo: 15,
            Imagens: [
                {
                    Data: "2025-04-18",
                    Base64Data: "data:image/jpeg;base64,...",
                    ProdutoId: ObjectId()
                }
            ]
        },
        {
            IdSequencial: 2,
            Nome: "Batata Frita",
            Preco: 8.99,
            Descricao: "Batata frita crocante",
            Tipo: "Acompanhamento",
            TempoPreparo: 10,
            Imagens: [
                {
                    Data: "2025-04-18",
                    Base64Data: "data:image/jpeg;base64,...",
                    ProdutoId: ObjectId()
                }
            ]
        },
        {
            IdSequencial: 3,
            Nome: "Refrigerante",
            Preco: 6.99,
            Descricao: "Refrigerante 600ml",
            Tipo: "Bebida",
            TempoPreparo: 1,
            Imagens: [
                {
                    Data: "2025-04-18",
                    Base64Data: "data:image/jpeg;base64,...",
                    ProdutoId: ObjectId()
                }
            ]
        }
    ]);
}