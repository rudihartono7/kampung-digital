using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;
using System.Security.Claims;
using Trisatech.KampDigi.Domain;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class ResidentBillController : Controller
{
    private readonly ILogger<ResidentBillController> _logger;
    private readonly IResidentBillService _residentBillService;
    public ResidentBillController(ILogger<ResidentBillController> logger,
    IResidentBillService residentBillService)
    {
        _logger = logger;
        _residentBillService = residentBillService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _residentBillService.Get(GetCurrentUserGuid());

        var model = new List<ResidentBillModel>();

        for(int i = 0; i < result.Count; i++)
        {
            model.Add(new ResidentBillBaseInfoModel
            {
                Id = result[i].Id,
                NameResident = result[i].NameResident,
                Nominal = result[i].Nominal,
                Status = result[i].Status,
            });
        }
        return View(model);
    }

    public Guid GetCurrentUserGuid()
    {
        Guid userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        return userId;
    }
    
}