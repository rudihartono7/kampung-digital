using System.Net.Http;
using System.Net;
using System.Collections.Concurrent;
using System.Reflection.Metadata;
using System.Net.Cache;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models.Bill;
using System.Security.Claims;
using Trisatech.KampDigi.Domain;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class ResidentBillController : BaseController
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
        var result = await _residentBillService.GetAll();

        var model = new List<ResidentBillModel>();

        for(int i = 0; i < result.Count; i++)
        {
            model.Add(new ResidentBillModel
            {
                Id = result[i].Id,
                ResidentTo = result[i].ResidentTo,
                Year = result[i].Year,
                Nominal = result[i].Nominal,
            });
        }
        return View(model);
    }
    
    public IActionResult Create() {
        return View(new ResidentBillModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ResidentBillModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try{
            var bill = request.ConvertToDbModel();
            // bill.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // for (int i = 0; i < HttpRequestException. i++)
            // {
            //     bill.ResidentTo = request.ResidentTo[i];
            // }
            await _residentBillService.Add(bill);
            
            return Redirect(nameof(Index));
        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return View(request);
    }

    public Guid GetCurrentUserGuid()
    {
        Guid userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        return userId;
    }
    
}