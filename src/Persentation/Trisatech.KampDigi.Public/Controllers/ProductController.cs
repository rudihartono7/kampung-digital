using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Public.Models;
using Trisatech.KampDigi.Public.Helpers;
using Trisatech.KampDigi.Application.Models;
using System.Security.Claims;

using Trisatech.KampDigi.Application;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class ProductController : BaseController
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;
    private readonly IWebHostEnvironment _iWebHost;

    public ProductController(ILogger<ProductController> logger,
    IProductService productService, IWebHostEnvironment iWebHost)
    {
        _logger = logger;
        _iWebHost = iWebHost;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _productService.GetId(GetCurrentUserGuid());
        return View(result);
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null){
            return BadRequest();
        }

        var produk = await _productService.DetailProduk(id.Value);

        if (produk == null)
        {
            return NotFound();
        }

        return View(produk);
    }

    public Guid GetCurrentUserGuid()
    {
        Guid userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData).Value);
        return userId;
    }


    
}
  