using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.WebApp.Models;
using System.Security.Claims;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Application.Models;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly KampDigiContext _digiContext;
    private readonly IGuestBookService _guestBookService;

    public HomeController(ILogger<HomeController> logger,
        KampDigiContext kampDigiContext,
        IGuestBookService guestBookService)
    {
        _logger = logger;
        _digiContext = kampDigiContext;
        _guestBookService = guestBookService;
    }

    public async Task<IActionResult> Index()
    {

        if (TempData["message"] != null)
        {
            ViewBag.Message = TempData["message"].ToString();
            TempData.Remove("message");
        }
        var guestList = await _guestBookService.GuestBookList();


        return View(new DashboardModel
        {
            Guests = guestList,
        });
    }
    
    public IActionResult ErrorAction()
    {

        if (TempData["message"] != null)
        {
            ViewBag.Message = TempData["message"].ToString();
            TempData.Remove("message");
        }
        return View();
    }

    public IActionResult Denied()
    {
        return View();
    }

    public IActionResult RedirectUser()
    {
        var dataResident = _digiContext.Users.FirstOrDefault(x => x.Id == GetCurrentUserGuid());
        return RedirectToActionPermanent("ResidentDetail", "Resident", new { id = dataResident.ResidentId });
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
    public Guid GetCurrentUserGuid()
    {
        Guid userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        return userId;
    }
}
