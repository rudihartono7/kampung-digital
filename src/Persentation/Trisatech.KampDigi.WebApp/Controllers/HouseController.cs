using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.WebApp.Models;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class HouseController : BaseController
{
   private readonly ILogger<HouseController> _logger;
   private readonly IHouseService _houseService;
   private readonly IResidenceService _residenceService;
private readonly KampDigiContext _digiContext;
   public HouseController(ILogger<HouseController> logger,
   IHouseService houseService, IResidenceService residenceService, KampDigiContext digiContext)
   {
      _logger = logger;
      _houseService = houseService;
      _residenceService = residenceService;
      _digiContext = digiContext;
   }

   public async Task<IActionResult> Index()
   {
      var result = await _houseService.GetAll();
      var house = new List<HouseModel>();
      var residence = await _residenceService.getData();
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
      ViewBag.ResidenceId = residence.Id;
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
         type = data.Type,
         residenceId = data.ResidenceId
      });
   }

   public async Task<IActionResult> Detail(Guid? Id)
   {
      if (Id == null)
      {
         return Json(new
         {
            success = false,
            message = "Invalid Operation"
         });
      }
      var data = await _houseService.DetailHouse(Id.Value);
      if (data == null)
      {
         return Json(new
         {
            success = false,
            message = "House Detail Not Found database"
         });
      }
      return View(new HouseDetailModel
      {
         Id = data.Id,
         Number = data.Number,
         Order = data.Order,
         Status = data.Status,
         Type = data.Type,
         ResidenceId = data.ResidenceId,
         HeadOfFamilyName = data.HeadOfFamilyName ?? "Belum memiliki pemilik",
         FamilyMember = data.FamilyMember
      });
   }

   public async Task<IActionResult> MyHouse (){
      Guid userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
      var resident = (from u in _digiContext.Users join r in _digiContext.Residents on u.ResidentId equals r.Id
                     where u.Id == userId
                     select new {
                        HouseId = r.HouseId
                     }).FirstOrDefault();
      var houseDet = await _houseService.DetailHouse(resident.HouseId);
      if (houseDet == null)
      {
         ViewBag.Error = "Resident belum memiliki rumah";
         return View();
      }
      return View(new HouseDetailModel
      {
         Id = houseDet.Id,
         Number = houseDet.Number,
         Order = houseDet.Order,
         Status = houseDet.Status,
         Type = houseDet.Type,
         ResidenceId = houseDet.ResidenceId,
         HeadOfFamilyName = houseDet.HeadOfFamilyName ?? "Belum memiliki pemilik",
         FamilyMember = houseDet.FamilyMember
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

