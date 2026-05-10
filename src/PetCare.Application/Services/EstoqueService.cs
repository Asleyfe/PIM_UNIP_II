using PetCare.Application.DTOs.Estoque;
using PetCare.Application.Services.Interfaces;
using PetCare.Domain.Entities.Estoque;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces.Repositories;

namespace PetCare.Application.Services;

public class EstoqueService : IEstoqueService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IMovimentacaoEstoqueRepository _movimentacaoRepository;

    public EstoqueService(
        IProdutoRepository produtoRepository,
        IMovimentacaoEstoqueRepository movimentacaoRepository)
    {
        _produtoRepository = produtoRepository;
        _movimentacaoRepository = movimentacaoRepository;
    }

    public async Task<IEnumerable<ProdutoResponseDto>> ListarTodosProdutos()
    {
        var produtos = await _produtoRepository.Listar();
        return produtos.Select(MapToDto);
    }

    public async Task<ProdutoResponseDto?> ObterProdutoPorId(long id)
    {
        var produto = await _produtoRepository.ObterPorId(id);
        return produto == null ? null : MapToDto(produto);
    }

    public async Task<ProdutoResponseDto> CadastrarProduto(ProdutoCreateDto dto)
    {
        var produto = new Produto(
            dto.Nome,
            dto.Preco,
            dto.Categoria,
            dto.EstoqueMinimo,
            dto.Descricao,
            dto.Validade
        );

        var inserido = await _produtoRepository.Inserir(produto);
        return MapToDto(inserido);
    }

    public async Task<bool> AtualizarProduto(long id, ProdutoCreateDto dto)
    {
        var existente = await _produtoRepository.ObterPorId(id);
        if (existente == null) return false;

        // Criar um novo objeto ou usar reflexão para atualizar, 
        // já que o domínio Produto é imutável via private setters.
        var atualizado = new Produto(
            dto.Nome,
            dto.Preco,
            dto.Categoria,
            dto.EstoqueMinimo,
            dto.Descricao,
            dto.Validade
        );
        atualizado.SetId(id);
        
        // Mantém a quantidade de estoque atual
        atualizado.AtualizarEstoque(existente.QuantidadeEstoque);

        return await _produtoRepository.Atualizar(atualizado);
    }

    public async Task<bool> RemoverProduto(long id)
    {
        return await _produtoRepository.Remover(id);
    }

    public async Task<bool> RegistrarMovimentacao(long produtoId, int quantidade, string tipo)
    {
        var produto = await _produtoRepository.ObterPorId(produtoId);
        if (produto == null) return false;

        if (!Enum.TryParse<TipoMovimentacao>(tipo, true, out var tipoEnum))
            throw new ArgumentException("Tipo de movimentação inválido. Use 'ENTRADA' ou 'SAIDA'.");

        var movimentacao = new MovimentacaoEstoque(produtoId, quantidade, tipoEnum);
        
        // Atualiza o estoque do produto
        int ajuste = (tipoEnum == TipoMovimentacao.Entrada) ? quantidade : -quantidade;
        produto.AtualizarEstoque(ajuste);

        // Persiste ambos
        await _movimentacaoRepository.Inserir(movimentacao);
        return await _produtoRepository.Atualizar(produto);
    }

    public async Task<IEnumerable<MovimentacaoEstoqueDto>> ListarMovimentacoesPorProduto(long produtoId)
    {
        var movs = await _movimentacaoRepository.ListarPorProduto(produtoId);
        return movs.Select(m => new MovimentacaoEstoqueDto
        {
            ProdutoId = m.ProdutoId,
            Quantidade = m.Quantidade,
            Tipo = m.Tipo.ToString(),
            DataMovimentacao = m.DataMovimentacao
        });
    }

    public async Task<IEnumerable<ProdutoResponseDto>> ListarProdutosEstoqueBaixo()
    {
        var produtos = await _produtoRepository.ListarComEstoqueBaixo();
        return produtos.Select(MapToDto);
    }

    public async Task<IEnumerable<ProdutoResponseDto>> ListarProdutosVencendo()
    {
        var produtos = await _produtoRepository.ListarProximosDoVencimento();
        return produtos.Select(MapToDto);
    }

    private static ProdutoResponseDto MapToDto(Produto p)
    {
        return new ProdutoResponseDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Preco = p.Preco,
            Descricao = p.Descricao,
            Categoria = p.Categoria,
            QuantidadeEstoque = p.QuantidadeEstoque,
            EstoqueMinimo = p.EstoqueMinimo,
            Validade = p.Validade,
            PrecisaReposicao = p.PrecisaReposição(),
            EstaVencido = p.EstaVencido(),
            ProximoAoVencimento = p.ProximoAoVencimento()
        };
    }
}
