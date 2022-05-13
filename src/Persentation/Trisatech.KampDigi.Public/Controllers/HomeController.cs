using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Public.Models;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Application;
using System.Security.Claims;

namespace Trisatech.KampDigi.Public.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductPublicService _productPublicService;
    private readonly IWebHostEnvironment _iWebHost;

    public HomeController(ILogger<HomeController> logger,
    IProductPublicService productPublicService, IWebHostEnvironment iWebHost)
    {
        _logger = logger;
        _iWebHost = iWebHost;
        _productPublicService = productPublicService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _productPublicService.GetAllProduct();
        return View(result);
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null){
            return BadRequest();
        }

        var produk = await _productPublicService.GetProduct(id.Value);

        if (produk == null)
        {
            return NotFound();
        }

        return View(produk);
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
