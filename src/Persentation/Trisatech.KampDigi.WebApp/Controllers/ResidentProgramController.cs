using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
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
      var result = "Halo ini halaman agenda";
      return View();
   }
}
