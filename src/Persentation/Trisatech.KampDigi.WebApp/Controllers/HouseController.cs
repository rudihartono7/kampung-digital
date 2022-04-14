using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models.Rumah;
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

   public async Task<IActionResult> Create()
   {
      return View(new HouseModel());
   }

   [HttpPost]
   public async Task<IActionResult> Create(HouseModel request)
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

         var house = request.ConvertToDbModel();
         await _houseService.Add(house);
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
         throw new Exception("Tidak ada Id yang dikirim ke server !");
      }
      var data = await _houseService.Get(id.Value);
      if (data == null)
      {
         throw new Exception("Data yang diminta tidak tersedia di server !");
      }
      return View(new HouseModel()
      {
         Id = data.Id,
         Number = data.Number,
         Order = data.Order,
         Status = data.Status,
         Type = data.Type
      });
   }

   [HttpPost]
   public async Task<IActionResult>EditRumah(HouseModel request)
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

         var house = request.ConvertToDbModel();
         await _houseService.Update(house);
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

