using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.WebApp.Models;
using Trisatech.KampDigi.WebApp.Helpers;
using Trisatech.KampDigi.Application.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Trisatech.KampDigi.Application;

namespace Trisatech.KampDigi.WebApp.Controllers;
[Authorize(Roles = AppConstant.RESIDENT)]
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


    public IActionResult Create()
    {
        return View(new ProductModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductModel request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }
        if (request == null)
        {
            return View(request);
        }
        try
        {
            string fileName = string.Empty;

            if (request.ImageFile != null)
            {
                fileName = $"{Guid.NewGuid()}-{request.ImageFile?.FileName}";

                string filePathName = _iWebHost.WebRootPath + $"/images/{fileName}";

                using (var streamWriter = System.IO.File.Create(filePathName))
                {
                    //await streamWriter.WriteAsync(Common.StreamToBytes(request.GambarFile.OpenReadStream()));
                    //using extension to convert stream to bytes
                    await streamWriter.WriteAsync(request.ImageFile.OpenReadStream().ToBytes());
                }
            }
           
            request.ImageUrl = $"images/{fileName}";
            await _productService.AddProduk(request,  GetCurrentUserGuid());
            TempData["message"] = "Data Produk berhasil ditambahkan";
            return RedirectToAction(nameof(Index));

        }
        catch (InvalidOperationException ex)
        {
            ViewBag.ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            throw;
        }
        return View(request);
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            throw new Exception("Tidak ada Id yang dikirim ke server !");
        }
        var data = await _productService.Get(id.Value);
        if (data == null)
        {
            throw new Exception("Data yang diminta tidak tersedia di server !");
        }
        return View(new ProductModel()
        {
            Id = data.Id,
            Name = data.Name,
            Price = data.Price,
            PublicLink = data.PublicLink,
            SellerName = data.SellerName,
            WhatsappNumber = data.WhatsappNumber,
            ImageUrl = data.ImageUrl,
            Desc = data.Desc
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductModel request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }
        if (request == null)
        {
            return View(request);
        }
        try
        {

            string fileName = string.Empty;

            if (request.ImageFile != null)
            {
                fileName = $"{Guid.NewGuid()}-{request.ImageFile?.FileName}";

                string filePathName = _iWebHost.WebRootPath + $"/images/{fileName}";

                using (var streamWriter = System.IO.File.Create(filePathName))
                {
                    //await streamWriter.WriteAsync(Common.StreamToBytes(request.GambarFile.OpenReadStream()));
                    //using extension to convert stream to bytes
                    await streamWriter.WriteAsync(request.ImageFile.OpenReadStream().ToBytes());
                }
            }

            if (request.ImageFile != null)
            {
                request.ImageUrl = $"images/{fileName}";
            }

            await _productService.EditProduk(request, GetCurrentUserGuid());
            TempData["message"] = "Data Produk berhasil diedit";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ViewBag.ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            throw;
        }
        return View(request);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
       
        await _productService.Delete(id);
        TempData["message"] = "Data Produk berhasil dihapus";
        return RedirectToAction("Index");

    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
