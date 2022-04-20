using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class MyResidentBillController : Controller
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
        return View();
    }
    
}