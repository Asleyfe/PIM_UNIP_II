using PetCare.Application.DTOs.Vendas;

namespace PetCare.Application.Services.Interfaces;

public interface IVendaService
{
    Task<IEnumerable<VendaResponseDto>> ListarTodas();
    Task<VendaResponseDto?> ObterPorId(long id);
    Task<IEnumerable<VendaResponseDto>> ListarPorPeriodo(DateTime inicio, DateTime fim);
    Task<VendaResponseDto> RealizarVenda(VendaCreateDto dto);
}
