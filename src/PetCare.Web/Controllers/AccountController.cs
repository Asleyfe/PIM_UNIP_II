using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PetCare.Infrastructure.Configuration;

namespace PetCare.Web.Controllers;

public class AccountController : Controller
{
    private readonly SupabaseSettings _supabaseSettings;

    public AccountController(IOptions<SupabaseSettings> supabaseSettings)
    {
        _supabaseSettings = supabaseSettings.Value;
    }

    [HttpGet]
    public IActionResult Login()
    {
        ViewBag.SupabaseUrl = _supabaseSettings.Url;
        ViewBag.SupabaseKey = _supabaseSettings.AnonKey;
        return View();
    }
}
