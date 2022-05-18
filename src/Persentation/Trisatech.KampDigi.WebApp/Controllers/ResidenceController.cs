using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Application.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trisatech.KampDigi.WebApp.Helpers;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class ResidenceController : BaseController
{
   private readonly ILogger<ReportController> _logger;
   private readonly IResidenceService _residenceService;
   private readonly IResidentService _residentService;
   private readonly KampDigiContext _digiContext;
   private readonly IWebHostEnvironment _iWebHost;
   public ResidenceController(ILogger<ReportController> logger,
   IResidenceService residenceService, IResidentService residentService,
   KampDigiContext kampDigiContext, IWebHostEnvironment iwebHost)
   {
      _logger = logger;
      _residenceService = residenceService;
      _residentService = residentService;
      _digiContext = kampDigiContext;
      _iWebHost = iwebHost;
   }

   [Authorize]

   public async Task<IActionResult> Index()
   {
      var residence = await _residenceService.getData();
      if (residence == null)
      {
         ViewBag.Error = "Data Desa tidak ditemukan";
      }
      return View(residence);
   }

   private async Task SetDataSource()
   {
      var data = await _residentService.GetList();

      ViewBag.PersonInCharge = data.Select(x => new SelectListItem
      {
         Value = x.Id.ToString(),
         Text = x.Name,
         Selected = false
      }).ToList();
   }

   private async Task SetDataSource(Guid? IdResident)
   {

      if (IdResident == null)
      {
         await SetDataSource();
         return;
      }
      var resident = await _residentService.GetList();
      ViewBag.PersonInCharge = resident
           .Select(x => new SelectListItem
           {
              Value = x.Id.ToString(),
              Text = x.Name,
              Selected = x.Id == IdResident ? true : false
           }).ToList();
   }

   public async Task<IActionResult> Edit(Guid Id)
   {
 
      var residence = await _residenceService.getData(Id);
      await SetDataSource(residence.PersonInCharge);
      return View(new ResidenceModel()
      {
         Id = residence.Id,
         Address = residence.Address,
         GMapLink = residence.GMapLink,
         ImageUrl = residence.ImageUrl,
         Latitude = residence.Latitude,
         Longitude = residence.Longitude,
         Name = residence.Name,
         PersonInCharge = residence.PersonInCharge,
      });
   }

   public async Task<IActionResult> Create()
   {
      await SetDataSource();
      return View();
   }

   [Authorize(Roles = AppConstant.ADMIN)]
   [HttpPost]
   public async Task<IActionResult> Create(ResidenceModel req)
   {
      if (!ModelState.IsValid)
      {

         await SetDataSource(req.PersonInCharge);
         return View(req);
      }

      try
      {
         string fileName = string.Empty;

         if (req.GambarFile != null)
         {
            fileName = $"{Guid.NewGuid()}-{req.GambarFile?.FileName}";

            string filePathName = _iWebHost.WebRootPath + "\\assets\\media\\img\\" + fileName;

            using (var streamWriter = System.IO.File.Create(filePathName))
            {
               //await streamWriter.WriteAsync(Common.StreamToBytes(rereq.GambarFile.OpenReadStream()));
               //using extension to convert stream to bytes
               await streamWriter.WriteAsync(req.GambarFile.OpenReadStream().ToBytes());
            }
            req.ImageUrl = $"img/{fileName}";
         }

         var residence = req.ConvertToDbModel();
         await _residenceService.Add(residence);
         return Redirect(nameof(Index));
      }
      catch (System.Exception)
      {

         throw;
      }
   }

   [Authorize(Roles = AppConstant.ADMIN)]
   // POST
   [HttpPost]
   public async Task<IActionResult> Edit(Guid? Id, ResidenceModel quest)
   {
      if (!ModelState.IsValid)
      {

         await SetDataSource(quest.PersonInCharge);
         return View(quest);
      }

      if (quest == null || Id == null)
      {
         await SetDataSource();
         return View(quest);
      }

      try
      {
         string fileName = string.Empty;
         var residence = await _residenceService.getData(quest.Id.Value);

         if (quest.GambarFile != null)
         {
            fileName = $"{Guid.NewGuid()}-{quest.GambarFile?.FileName}";

            string filePathName = _iWebHost.WebRootPath + "\\assets\\media\\img\\" + fileName;

            string path = _iWebHost.WebRootPath + "\\assets\\media\\" + residence.ImageUrl;
            // hapus foto sebelumnya di direktori
            if (DeleteFile(path))
            {
               using (var streamWriter = System.IO.File.Create(filePathName))
               {
                  //await streamWriter.WriteAsync(Common.StreamToBytes(request.GambarFile.OpenReadStream()));
                  //using extension to convert stream to bytes
                  await streamWriter.WriteAsync(quest.GambarFile.OpenReadStream().ToBytes());
               }
               quest.ImageUrl = $"img/{fileName}";
            }
         }
         Guid userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
         quest.ImageUrl = string.IsNullOrEmpty(quest.ImageUrl) ? residence.ImageUrl : $"img/{fileName}";
         await _residenceService.modify(quest.ConvertToDbModel(), userId);

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

      await SetDataSource(quest.PersonInCharge);
      return View(quest);
   }

   public bool DeleteFile(string path)
   {
      // Delete a file by using File class static method...
      if (System.IO.File.Exists(path))
      {
         // Use a try block to catch IOExceptions, to
         // handle the case of the file already being
         // opened by another process.
         try
         {
            System.IO.File.Delete(path);
            return true;
         }
         catch (System.IO.IOException e)
         {
            Console.WriteLine(e.Message);
            return false;
         }
      }
      return false;
   }
}