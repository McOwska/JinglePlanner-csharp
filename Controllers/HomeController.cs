using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JinglePlanner.Models;
using Microsoft.AspNetCore.Authorization;

namespace JinglePlanner.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        HttpContext.Session.Clear();
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
    
    public IActionResult Logged()
    {
        //check if user is logged in
        if(HttpContext.Session.GetString("IsLoggedIn") != null)
        {
            return View();
        }
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Admin()
    {
        if(HttpContext.Session.GetString("IsAdmin") == "true")
        {
            return View();
        }
        return RedirectToAction("Index", "Home");
    }

}
