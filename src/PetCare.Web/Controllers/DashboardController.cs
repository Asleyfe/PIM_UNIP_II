using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Services.Interfaces;

namespace PetCare.Web.Controllers;

[Route("Dashboard")]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("/api/Dashboard/indicadores")]
    public async Task<IActionResult> ObterIndicadores()
    {
        var indicadores = await _dashboardService.ObterIndicadores();
        return Ok(indicadores);
    }
}
