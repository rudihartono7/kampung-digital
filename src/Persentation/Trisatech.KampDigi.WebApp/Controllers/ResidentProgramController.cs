using System.Diagnostics;
using System.Security.Claims;
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
   [Authorize]

   public async Task<IActionResult> Index()
   {
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

   public async Task<IActionResult> GetData()
   {
      // string query = $"SELECT * FROM residentprograms WHERE EXTRACT(MONTH FROM StartDate) = {DateTime.Now.Month}";
      string query = $"SELECT * FROM residentprograms";
      var result = await _residentProgramService.GetByQuery(query);
      return Json(new { data = result });
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
         request.CreatedBy = GetCurrentUserGuid();
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

   public async Task<IActionResult> Detail(Guid Id)
   {
      var data = await _residentProgramService.GetDetail(Id);
      if (data == null)
      {
         return Json(new
         {
            success = 0,
            msg = "data tidak ditemukan"
         });
      }
      return Json(new { success = 1, data = data });
   }

   public async Task<IActionResult> Edit(Guid id)
   {
      var data = await _residentProgramService.GetDetail(id);
      await SetKategoriDataSource(data.PersonInChargeId);
      if (data == null)
      {
         return View();
      }
      return View(data);
   }

   [HttpPost]
   public async Task<IActionResult> Edit(Guid Id, ResidentProgramModel request)
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
         request.UpdatedBy = GetCurrentUserGuid();
         var data = request.ConvertToDbModel();
         await _residentProgramService.Update(data);
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

   [HttpPost]
   public async Task<IActionResult> Delete(Guid? id)
   {
      if (id == null)
      {
         return Json(new
         {
            success = false,
            message = "Data Rumah tidak ditemukan"
         });
      }
      try
      {

         await _residentProgramService.Delete(id.Value);

         return Json(new
         {
            success = true
         });
      }
      catch (InvalidOperationException ex)
      {
         return Json(new
         {
            success = false,
            msg = ex.Message
         });
      }
      catch
      {
         throw;
      }
   }

   public Guid GetCurrentUserGuid()
   {
      Guid userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
      return userId;
   }
}
