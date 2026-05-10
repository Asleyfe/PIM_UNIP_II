using PetCare.Application.DTOs.Vendas;
using PetCare.Application.Services.Interfaces;
using PetCare.Domain.Entities.Vendas;
using PetCare.Domain.Interfaces.Repositories;

namespace PetCare.Application.Services;

public class VendaService : IVendaService
{
    private readonly IVendaRepository _vendaRepository;
    private readonly ITutorRepository _tutorRepository;
    private readonly IProdutoRepository _produtoRepository;

    public VendaService(
        IVendaRepository vendaRepository,
        ITutorRepository tutorRepository,
        IProdutoRepository produtoRepository)
    {
        _vendaRepository = vendaRepository;
        _tutorRepository = tutorRepository;
        _produtoRepository = produtoRepository;
    }

    public async Task<IEnumerable<VendaResponseDto>> ListarTodas()
    {
        var vendas = await _vendaRepository.Listar();
        var dtos = new List<VendaResponseDto>();
        foreach (var v in vendas)
        {
            dtos.Add(await MapToDto(v));
        }
        return dtos;
    }

    public async Task<VendaResponseDto?> ObterPorId(long id)
    {
        var venda = await _vendaRepository.ObterPorId(id);
        return venda == null ? null : await MapToDto(venda);
    }

    public async Task<IEnumerable<VendaResponseDto>> ListarPorPeriodo(DateTime inicio, DateTime fim)
    {
        var vendas = await _vendaRepository.ListarPorPeriodo(inicio, fim);
        var dtos = new List<VendaResponseDto>();
        foreach (var v in vendas)
        {
            dtos.Add(await MapToDto(v));
        }
        return dtos;
    }

    public async Task<VendaResponseDto> RealizarVenda(VendaCreateDto dto)
    {
        if (await _tutorRepository.ObterPorId(dto.TutorId) == null)
            throw new ArgumentException($"Tutor com id {dto.TutorId} não encontrado.");

        if (dto.Itens == null || !dto.Itens.Any())
            throw new ArgumentException("A venda deve possuir pelo menos um item.");

        var venda = new Venda(dto.TutorId, dto.FormaPagamento, dto.Observacao);
        var itens = new List<ItemVenda>();

        foreach (var itemDto in dto.Itens)
        {
            var produto = await _produtoRepository.ObterPorId(itemDto.ProdutoId);
            if (produto == null)
                throw new ArgumentException($"Produto com id {itemDto.ProdutoId} não encontrado.");

            if (produto.QuantidadeEstoque < itemDto.Quantidade)
                throw new InvalidOperationException($"Estoque insuficiente para o produto {produto.Nome}. Disponível: {produto.QuantidadeEstoque}");

            var item = new ItemVenda(0, itemDto.ProdutoId, itemDto.Quantidade, itemDto.PrecoUnitario);
            itens.Add(item);
            
            // Adiciona ao objeto venda para cálculo do total (mesmo que o repositório use a lista itens)
            venda.AdicionarItem(itemDto.ProdutoId, itemDto.Quantidade, itemDto.PrecoUnitario);
        }

        var inserida = await _vendaRepository.InserirComItens(venda, itens);
        return await MapToDto(inserida);
    }

    private async Task<VendaResponseDto> MapToDto(Venda v)
    {
        var tutor = await _tutorRepository.ObterPorId(v.TutorId);
        
        var dto = new VendaResponseDto
        {
            Id = v.Id,
            TutorId = v.TutorId,
            TutorNome = tutor?.Nome ?? "N/A",
            DataVenda = v.DataVenda,
            ValorTotal = v.ValorTotal,
            FormaPagamento = v.FormaPagamento,
            Observacao = v.Observacao,
            CreatedAt = v.CreatedAt
        };

        if (v.Itens != null)
        {
            foreach (var item in v.Itens)
            {
                var produto = await _produtoRepository.ObterPorId(item.ProdutoId);
                dto.Itens.Add(new ItemVendaResponseDto
                {
                    Id = item.Id,
                    ProdutoId = item.ProdutoId,
                    ProdutoNome = produto?.Nome ?? "N/A",
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.PrecoUnitario
                });
            }
        }

        return dto;
    }
}
