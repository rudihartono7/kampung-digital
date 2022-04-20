using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.WebApp.Models;
using Trisatech.KampDigi.Application.Models.Account;
using Trisatech.KampDigi.Domain.Entities;
using Trisatech.KampDigi.Application;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Trisatech.KampDigi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class ResidenceController : BaseController
{
   private readonly ILogger<ReportController> _logger;
   private readonly IResidenceService _residenceService;
   private readonly KampDigiContext _digiContext;
   
   public ResidenceController(ILogger<ReportController> logger,
   IResidenceService residenceService,
   KampDigiContext kampDigiContext)
   {
      _logger = logger;
      _residenceService = residenceService;
      _digiContext = kampDigiContext;
   }

   [Authorize(Roles = AppConstant.ADMIN)]

   public async Task<IActionResult> Index (){
      var residence = await _residenceService.getData();
      if (residence == null)
      {
         ViewBag.Error = "Data Desa tidak ditemukan";
      }
      return View(residence);
   }
}