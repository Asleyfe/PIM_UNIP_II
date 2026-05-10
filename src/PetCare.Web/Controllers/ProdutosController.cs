using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Estoque;
using PetCare.Application.Services.Interfaces;

namespace PetCare.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IEstoqueService _estoqueService;

    public ProdutosController(IEstoqueService estoqueService)
    {
        _estoqueService = estoqueService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var produtos = await _estoqueService.ListarTodosProdutos();
        return Ok(produtos);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var produto = await _estoqueService.ObterProdutoPorId(id);
        if (produto is null)
            return NotFound(new { mensagem = $"Produto com id {id} não encontrado." });

        return Ok(produto);
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] ProdutoCreateDto dto)
    {
        try
        {
            var produto = await _estoqueService.CadastrarProduto(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, produto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Atualizar(long id, [FromBody] ProdutoCreateDto dto)
    {
        var atualizado = await _estoqueService.AtualizarProduto(id, dto);
        if (!atualizado)
            return NotFound(new { mensagem = $"Produto com id {id} não encontrado." });

        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Remover(long id)
    {
        var removido = await _estoqueService.RemoverProduto(id);
        if (!removido)
            return NotFound(new { mensagem = $"Produto com id {id} não encontrado." });

        return NoContent();
    }

    [HttpGet("estoque-baixo")]
    public async Task<IActionResult> ListarEstoqueBaixo()
    {
        var produtos = await _estoqueService.ListarProdutosEstoqueBaixo();
        return Ok(produtos);
    }

    [HttpGet("vencendo")]
    public async Task<IActionResult> ListarVencendo()
    {
        var produtos = await _estoqueService.ListarProdutosVencendo();
        return Ok(produtos);
    }

    [HttpPost("{id:long}/movimentacao")]
    public async Task<IActionResult> RegistrarMovimentacao(long id, [FromBody] MovimentacaoRequest request)
    {
        try
        {
            var sucesso = await _estoqueService.RegistrarMovimentacao(id, request.Quantidade, request.Tipo);
            if (!sucesso)
                return NotFound(new { mensagem = $"Produto com id {id} não encontrado." });

            return Ok(new { mensagem = "Movimentação registrada com sucesso." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpGet("{id:long}/movimentacoes")]
    public async Task<IActionResult> ListarMovimentacoes(long id)
    {
        var movs = await _estoqueService.ListarMovimentacoesPorProduto(id);
        return Ok(movs);
    }
}

public class MovimentacaoRequest
{
    public int Quantidade { get; set; }
    public string Tipo { get; set; } = string.Empty;
}
