using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;

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


    // public async Task<IActionResult> Index(Guid? idResident)
    // {
    //     var result = await _residentBillService.Get();

    //     var model = new List<ResidentBillModel>();

    //     for(int i = 0; i < result.Count; i++)
    //     {
    //         model.Add(new ResidentBillModel
    //         {
    //             Id = result[i].Id,
    //             NameResident = result[i].NameResident,
    //             PaymentDate = result[i].PaymentDate,
    //             Nominal = result[i].Nominal,
    //             Status = result[i].Status
    //         });
    //     }
    //     return View(model);
    // }

    public async Task<IActionResult> Index()
    {
        var result = await _residentBillService.GetAll();

        var model = new List<ResidentBillModel>();

        for(int i = 0; i < result.Count; i++)
        {
            model.Add(new ResidentBillModel
            {
                Id = result[i].Id,
                NameResident = result[i].NameResident,
                PaymentDate = result[i].PaymentDate,
                Nominal = result[i].Nominal,
                Status = result[i].Status
            });
        }
        return View(model);
    }
    
}