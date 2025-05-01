using MediatR;
using Application.DTOs;

namespace Application.Queries
{ 
    public class ObterTodosProdutosQuery : IRequest<List<ProdutoDto>>
    {
    }
}