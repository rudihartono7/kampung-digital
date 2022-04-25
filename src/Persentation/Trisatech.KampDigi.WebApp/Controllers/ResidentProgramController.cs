using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.WebApp.Models;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class ResidentProgramController : BaseController
{
   private readonly ILogger<ResidentProgramController> _logger;
   private readonly IResidentProgramService _residentProgramService;
   private readonly KampDigiContext _digiContext;
   public ResidentProgramController(ILogger<ResidentProgramController> logger,
   IResidentProgramService residentProgramService, KampDigiContext digiContext)
   {
      _logger = logger;
      _residentProgramService = residentProgramService;
      _digiContext = digiContext;
   }
   // [Authorize(Roles = AppConstant.ADMIN)]

   public async Task<IActionResult> Index()
   {
      // var result = await _residentProgramService.GetAll();
      // var data = new List<ResidentProgramModel>();
      // foreach (var item in result)
      // {
      //    data.Add(new ResidentProgramModel{
      //       Id = item.Id,
      //       Year = item.Year,
      //       Cost = item.Cost,
      //       Desc = item.Desc,
      //       EndDate = item.EndDate,
      //       PersonInChargeId = item.PersonInChargeId,
      //       ProgramPeriod = item.ProgramPeriod,
      //       ProgramSubject = item.ProgramSubject,
      //       StartDate = item.StartDate,
      //       Title = item.Title
      //    });
      // }
      return View();
   }

   private async Task SetKategoriDataSource()
   {
      var residents = await _digiContext.Residents.ToListAsync();

      ViewBag.ResidentDataSource = residents.Select(x => new SelectListItem
      {
         Value = x.Id.ToString(),
         Text = x.Name,
         Selected = false
      }).ToList();
   }

   private async Task SetKategoriDataSource(Guid PersonInChargeId)
   {

      if (PersonInChargeId == null)
      {
         await SetKategoriDataSource();
         return;
      }
      var residents = await _digiContext.Residents.ToListAsync();
      ViewBag.ResidentDataSource = residents.Select(x =>
      new SelectListItem
      {
         Value = x.Id.ToString(),
         Text = x.Name,
         Selected = residents.FirstOrDefault(y => y.Id == x.Id) == null ? false : true
      }).ToList();
   }


   public async Task<IActionResult> Create()
   {
      await SetKategoriDataSource();
      return View(new ResidentProgramModel());
   }

   [HttpPost]
   public async Task<IActionResult> Create(ResidentProgramModel request)
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
         var data = request.ConvertToDbModel();
         await _residentProgramService.Add(data);
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
}
