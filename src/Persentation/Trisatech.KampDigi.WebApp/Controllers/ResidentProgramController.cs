using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models.ResidentProgram;
using Trisatech.KampDigi.WebApp.Models;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class ResidentProgramController : Controller
{
   private readonly ILogger<ResidentProgramController> _logger;
   private readonly IResidentProgramService _residentProgramService;
   public ResidentProgramController(ILogger<ResidentProgramController> logger,
   IResidentProgramService residentProgramService)
   {
      _logger = logger;
      _residentProgramService = residentProgramService;
   }

   public async Task<IActionResult> Index()
   {
      var result = await _residentProgramService.GetAll();
      return View(result);
   }

   public async Task<IActionResult> Create(){
      return View(new ResidentProgramModel());
   }

   [HttpPost]
   public async Task<IActionResult> Create(ResidentProgramModel request){
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
         request.Id = new Guid();

         await _residentProgramService.Add(request);
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
