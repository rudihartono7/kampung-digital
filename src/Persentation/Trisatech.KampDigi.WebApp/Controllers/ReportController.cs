using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.WebApp.Models;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class ReportController : Controller
{
    private readonly ILogger<ReportController> _logger;
    private readonly IResidentFundService _residentFundService;
    public ReportController(ILogger<ReportController> logger,
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

    public async Task<IActionResult> History()
    {
        var result = await _residentFundService.GetCurrentBalance();

        ViewBag.TestingResidentFund = result;
        return View();
    }

}
