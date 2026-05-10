using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PetCare.Infrastructure.Configuration;
using PetCare.Web.Models;

namespace PetCare.Web.Controllers;

public class HomeController : Controller
{
    private readonly SupabaseSettings _supabaseSettings;

    public HomeController(IOptions<SupabaseSettings> supabaseSettings)
    {
        _supabaseSettings = supabaseSettings.Value;
    }

    public IActionResult Index()
    {
        ViewBag.SupabaseUrl = _supabaseSettings.Url;
        ViewBag.SupabaseKey = _supabaseSettings.AnonKey;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
