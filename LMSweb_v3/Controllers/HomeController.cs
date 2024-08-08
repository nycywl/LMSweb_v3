using LMSweb_v3.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using LMSwebDB.Helper;
using LMSwebDB.Repositories;
using LMSwebDB.Models;

namespace LMSweb_v3.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly LMSRepository _context;

    public HomeController(ILogger<HomeController> logger, LMSRepository context)
    {
        _logger = logger;
        _context = context;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }


    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel login)
    {
        if (ModelState.IsValid)
        {
            var loginUser = await _context.Query<User>()
                .FirstOrDefaultAsync(u => u.UserId == login.ID && u.Upassword == HashHelper.SHA256Hash(login.Password));
            if (loginUser != null)
            {
                List<Claim> claims = [
                    new Claim(ClaimTypes.Name, loginUser.Name),
                    new Claim(ClaimTypes.Role, loginUser.RoleName),
                    new Claim("UID", loginUser.UserId),
                ];
                
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Home", loginUser.RoleName);
            }
            ViewBag.isError = true;
            return View();
        }
        return View(login);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}