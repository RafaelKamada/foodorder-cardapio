using Application.Repositories.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MongoDB.Bson;

namespace Tests.Repositories
{
    public class ProdutoRepositoryEmMemoria : IProdutoRepository
    {
        private readonly List<Produto> _produtos = new List<Produto>();
        private int _nextSequencial = 1;

        private string GerarIdMongo()
        {
            return ObjectId.GenerateNewId().ToString();
        }

        public async Task<Produto> ObterPorIdAsync(int id)
        {
            return await Task.FromResult(_produtos.FirstOrDefault(p => p.IdSequencial == id));
        }

        public async Task<List<Produto>> ObterPorIdsAsync(List<int> ids)
        {
            return await Task.FromResult(_produtos.Where(p => ids.Contains(p.IdSequencial)).ToList());
        }

        public async Task<List<Produto>> ObterTodosAsync()
        {
            return await Task.FromResult(_produtos.ToList());
        }

        public async Task<List<Produto>> ObterPorCategoriaAsync(CategoriaTipo categoria)
        {
            return await Task.FromResult(_produtos.Where(p => p.Tipo == categoria.ToString()).ToList());
        }

        public async Task<Produto> CriarAsync(Produto produto)
        {
            produto.IdSequencial = _nextSequencial++;
            _produtos.Add(produto);
            return produto;
        }

        public async Task<Produto> AtualizarAsync(Produto produto)
        {
            var index = _produtos.FindIndex(p => p.IdSequencial == produto.IdSequencial);
            if (index != -1)
            {
                _produtos[index] = produto;
            }
            return produto;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var produto = _produtos.FirstOrDefault(p => p.IdSequencial == id);
            if (produto != null)
            {
                _produtos.Remove(produto);
                return true;
            }
            return false;
        }
    }
}