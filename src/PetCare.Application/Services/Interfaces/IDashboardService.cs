using PetCare.Application.DTOs.Dashboard;

namespace PetCare.Application.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardResponseDto> ObterIndicadores();
}
