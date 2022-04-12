using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.WebApp.Models;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IResidentFundService _residentFundService;
    public HomeController(ILogger<HomeController> logger,
    IResidentFundService residentFundService)
    {
        _logger = logger;
        _residentFundService = residentFundService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _residentFundService.GetCurrentBalance();

        ViewBag.TestingResidentFund = result;
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
