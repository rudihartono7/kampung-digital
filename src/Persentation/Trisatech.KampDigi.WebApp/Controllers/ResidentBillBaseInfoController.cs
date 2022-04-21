using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class ResidentBillBaseInfoController : BaseController
{
    private readonly ILogger<ResidentBillBaseInfoController> _logger;
    private readonly IResidentBillBaseInfoService _residentBillBaseInfoService;
    public ResidentBillBaseInfoController(ILogger<ResidentBillBaseInfoController> logger,
    IResidentBillBaseInfoService residentBillBaseInfoService)
    {
        _logger = logger;
        _residentBillBaseInfoService = residentBillBaseInfoService;
    }


    public async Task<IActionResult> Index()
    {
        var result = await _residentBillBaseInfoService.GetAll();

        var model = new List<ResidentBillBaseInfoModel>();

        for(int i = 0; i < result.Count; i++)
        {
            model.Add(new ResidentBillBaseInfoModel
            {
                Id = result[i].Id,
                Year = result[i].Year,
                Nominal = result[i].Nominal,
                MontlyBillOpenDate = result[i].MontlyBillOpenDate,
                DueDateNumber = result[i].DueDateNumber
            });
        }
        return View(model);
    }

    public IActionResult Create() {
        return View(new ResidentBillBaseInfoModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ResidentBillBaseInfoModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try{
            await _residentBillBaseInfoService.Add(request.ConvertToDbModel());

            return Redirect(nameof(Index));
        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return View(request);
    }

    public async Task<IActionResult> Edit(Guid? id){
        if(id == null)
        {
            return NotFound();
        }

        var result = await _residentBillBaseInfoService.Get(id.Value);

        if(result == null)
        {
            return NotFound();
        }

        return View(new ResidentBillBaseInfoModel(result));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid? id, ResidentBillBaseInfoModel request)
    {
        if (id == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _residentBillBaseInfoService.Update(request.ConvertUpdateToDbModel());

            return Redirect(nameof(Index));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return View(request);
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var result = await _residentBillBaseInfoService.Get(id.Value);

        if (result == null)
        {
            return NotFound();
        }

        return View(new ResidentBillBaseInfoModel(result));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid? id, ResidentBillBaseInfoModel request)
    {
        if (id == null)
        {
            return BadRequest();
        }

        try
        {
            await _residentBillBaseInfoService.Delete(id.Value);

            return Redirect(nameof(Index));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return View(request);
    }
    
}