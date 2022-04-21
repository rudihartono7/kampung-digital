using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models.Bill;
using System.Security.Claims;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class MyResidentBillController : BaseController
{
    private readonly ILogger<MyResidentBillController> _logger;
    private readonly IResidentBillService _residentBillService;
    public MyResidentBillController(ILogger<MyResidentBillController> logger,
    IResidentBillService residentBillService)
    {
        _logger = logger;
        _residentBillService = residentBillService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _residentBillService.Get(GetCurrentUserGuid());

        return View();
    }

    public Guid GetCurrentUserGuid()
    {
        Guid userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        return userId;
    }
    
}