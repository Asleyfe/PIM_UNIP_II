using PetCare.Application.DTOs.Estoque;

namespace PetCare.Application.Services.Interfaces;

public interface IEstoqueService
{
    Task<IEnumerable<ProdutoResponseDto>> ListarTodosProdutos();
    Task<ProdutoResponseDto?> ObterProdutoPorId(long id);
    Task<ProdutoResponseDto> CadastrarProduto(ProdutoCreateDto dto);
    Task<bool> AtualizarProduto(long id, ProdutoCreateDto dto);
    Task<bool> RemoverProduto(long id);
    
    Task<bool> RegistrarMovimentacao(long produtoId, int quantidade, string tipo);
    Task<IEnumerable<MovimentacaoEstoqueDto>> ListarMovimentacoesPorProduto(long produtoId);
    
    Task<IEnumerable<ProdutoResponseDto>> ListarProdutosEstoqueBaixo();
    Task<IEnumerable<ProdutoResponseDto>> ListarProdutosVencendo();
}
