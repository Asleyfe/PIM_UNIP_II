using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Services.Interfaces;

namespace PetCare.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("indicadores")]
    public async Task<IActionResult> ObterIndicadores()
    {
        var indicadores = await _dashboardService.ObterIndicadores();
        return Ok(indicadores);
    }
}
