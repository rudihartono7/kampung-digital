using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.WebApp.Models;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class HouseController : Controller
{
   private readonly ILogger<HouseController> _logger;
   private readonly IHouseService _houseService;
   public HouseController(ILogger<HouseController> logger,
   IHouseService houseService)
   {
      _logger = logger;
      _houseService = houseService;
   }

   public async Task<IActionResult> Index()
   {
      var result = await _houseService.GetAll();
      var house = new List<HouseModel>();
      foreach (var item in result)
      {
         house.Add(new HouseModel
         {
            Id = item.Id,
            Number = item.Number,
            Order = item.Order,
            Status = item.Status,
            Type = item.Type
         });
      }
      ViewBag.DaftarRumah = house;
      return View();
   }

   public IActionResult Create()
   {
      return View(new HouseModel());
   }

   [HttpPost]
   public async Task<IActionResult> Create(HouseModel request)
   {
      if (!ModelState.IsValid)
      {
         return Json(new
         {
            success = false,
            message = "Model invalid!"
         });
      }
      if (request == null)
      {
         return Json(new
         {
            success = false,
            message = "request kosong!"
         });
      }
      try
      {

         var house = request.ConvertToDbModel();
         await _houseService.Add(house);
         return Json(new
         {
            success = true,
            message = "Data berhasil disimpan!"
         });
      }
      catch (InvalidOperationException ex)
      {
         return Json(new
         {
            success = false,
            message = ex
         });
      }
      catch (Exception)
      {
         throw;
      }
   }

   public async Task<IActionResult> Edit(Guid? id)
   {
      if (id == null)
      {
         return Json(new
         {
            success = false,
            message = "Invalid Operation"
         });
      }
      var data = await _houseService.Get(id.Value);
      if (data == null)
      {
         return Json(new
         {
            success = false,
            message = "Rumah tidak ditemukan di database"
         });
      }
      return Json(new
      {
         success = true,
         id = data.Id,
         number = data.Number,
         order = data.Order,
         status = data.Status,
         type = data.Type
      });
   }

   [HttpPost]
   public async Task<IActionResult> EditRumah(HouseModel request)
   {
      if (!ModelState.IsValid)
      {
         return Json(new
         {
            success = false,
            message = "Model invalid!"
         });
      }
      if (request == null)
      {
         return Json(new
         {
            success = false,
            message = "request kosong!"
         });
      }
      try
      {

         var house = request.ConvertToDbModel();
         await _houseService.Update(house);
         return Json(new
         {
            success = true,
            message = "Proses Edit berhasil disimpan!"
         });
      }
      catch (InvalidOperationException ex)
      {
         return Json(new
         {
            success = false,
            message = ex
         });
      }
      catch (Exception)
      {
         throw;
      }
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

         await _houseService.Delete(id.Value);

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
            message = ex.Message
         });
      }
      catch
      {
         throw;
      }
   }
}

