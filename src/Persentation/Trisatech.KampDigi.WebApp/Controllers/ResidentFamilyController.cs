using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.WebApp.Models;
using Trisatech.KampDigi.WebApp.Helpers;
using Trisatech.KampDigi.Application.Models.ResidentFamilies;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trisatech.KampDigi.Domain;
using Microsoft.AspNetCore.Authorization;
using Trisatech.KampDigi.Application;

namespace Trisatech.KampDigi.WebApp.Controllers;

[Authorize(Roles = AppConstant.RESIDENT)]
public class ResidentFamilyController : BaseController
{
    private readonly ILogger<ResidentFamilyController> _logger;
    private readonly IResidentFamilyService _residentFamilyService;
    private readonly IResidentService _residentService;
    private readonly KampDigiContext _digiContext;
    public ResidentFamilyController(ILogger<ResidentFamilyController> logger,
    IResidentFamilyService residentFamilyService, IResidentService residentService,
    KampDigiContext digiContext)
    {
        _logger = logger;
        _residentFamilyService = residentFamilyService;
        _residentService = residentService;
        _digiContext = digiContext;
    }


    public async Task<IActionResult> Index()
    {
        ViewBag.HeadOfFamilyId = new SelectList(_digiContext.Residents,"Id","Name");
        var result = await _residentFamilyService.GetId(GetCurrentUserGuid());
        return View(result);

    }

    
    public Guid GetCurrentUserGuid()
    {
        Guid userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData).Value);
        return userId;
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null){
            return BadRequest();
        }

        var produk = await _residentFamilyService.FamilyDetail(id.Value);

        if (produk == null)
        {
            return NotFound();
        }

        return View(produk);
    }



     public IActionResult Create()
    {
        ViewBag.HeadOfFamilyId = new SelectList(_digiContext.Residents,"Id","Name");
        return View(new ResidentFamilyModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(ResidentFamilyModel request)
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
            await _residentFamilyService.FamilyAdd(request, GetCurrentUserGuid());

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
            return BadRequest();
        }
        var update = await _residentFamilyService.Get(id.Value);
        if (update == null)
        {
            return NotFound();
        }
        ViewBag.HeadOfFamilyId = new SelectList(_digiContext.Residents,"Id","Name");
        return View(new ResidentFamilyModel(){
                Id = update.Id,
                Name = update.Name,
                Gender=update.Gender,
                Age = update.Age,
                Relationship = update.Relationship,
                HeadOfFamilyId = update.HeadOfFamilyId
        });

    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid? id, ResidentFamilyModel request)
    {
        if (id == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(request);
        }


        try
        {
            await _residentFamilyService.FamilyEdit(request,GetCurrentUserGuid());
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

     public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        var delete = await _residentFamilyService.Get(id.Value);
        if (delete == null)
        {
            return NotFound();
        }
        return View(new ResidentFamilyModel(delete));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid? id,ResidentFamilyModel request)
    {
        if (id == null)
        {
            return BadRequest();
        }
        try
        {
            await _residentFamilyService.FamilyDelete(id.Value);

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


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
