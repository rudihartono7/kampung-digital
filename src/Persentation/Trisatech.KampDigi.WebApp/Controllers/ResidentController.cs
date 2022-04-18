using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Models.Resident;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.WebApp.Helpers;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Domain;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Trisatech.KampDigi.Application;

namespace Trisatech.KampDigi.WebApp.Controllers
{
    public class ResidentController : BaseController
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly IResidentService _residentService;
        private readonly KampDigiContext _digiContext;
        public ResidentController(IWebHostEnvironment webHost, 
            IResidentService residentService,
            KampDigiContext digiContext)
        {
            _webHost = webHost;
            _residentService = residentService;
            _digiContext = digiContext;
        }

        [Authorize(Roles = AppConstant.ADMIN)]
        public async Task<IActionResult> Index()
        {
            var listUser = await _residentService.GetList();

            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"].ToString();
                TempData.Remove("message");
            }
            ViewBag.House = new SelectList(_digiContext.Houses, "Id", "Number");
            return View(listUser);
        }

        [Authorize(Roles = AppConstant.ADMIN)]
        public IActionResult ResidentAdd()
        {
            ViewBag.House = new SelectList(_digiContext.Houses, "Id", "Number");
            return View(new ResidentAddModel());
        }

        [Authorize(Roles = AppConstant.ADMIN)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResidentAdd(ResidentAddModel dataResident)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Data input tidak valid. Pastikan data sudah terisi lengkap.";
                return View(dataResident);
            }
            try
            {

                dataResident.IdentityPhoto = await SaveFile(dataResident.KTP);
                dataResident.IdentityFamilyPhoto = await SaveFile(dataResident.KK);


                await _residentService.ResidentAdd(dataResident, GetCurrentUserGuid());

                TempData["message"] = "Data warga berhasil ditambahkan";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            catch (Exception)
            {
                throw;
            }

            return View(dataResident);
        }

        [Authorize]
        public async Task<IActionResult> ResidentDetail(Guid id)
        {
            if (_digiContext.Residents.Find(id) == null)
            {
                TempData["message"] = "Data warga tidak ditemukan. Apakah data warga sudah di input?";
                return RedirectToAction("ErrorAction", "Home");
            }

            ViewBag.House = new SelectList(_digiContext.Houses, "Id", "Number");

            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"].ToString();
                TempData.Remove("message");
            }
            return View(await _residentService.ResidentDetail(id));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResidentEdit(ResidentEditModel dataResident)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Data input tidak valid. Pastikan data sudah terisi lengkap.";
                return RedirectToAction("ErrorAction", "Home");
            }
            try
            {
                if (dataResident.KTP != null)
                {
                    dataResident.IdentityPhoto = await SaveFile(dataResident.KTP);
                }
                
                if (dataResident.KK != null)
                {
                    dataResident.IdentityFamilyPhoto = await SaveFile(dataResident.KK);
                }
                
                await _residentService.ResidentEdit(dataResident, GetCurrentUserGuid());

                TempData["message"] = "Data bewrhasil di ubah.";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            catch (Exception)
            {
                throw;
            }

            return View(dataResident);
        }


        [Authorize(Roles = AppConstant.ADMIN)]
        [HttpPost, ActionName("ResidentDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResidentDelete(Guid id)
        {
            var dataResident = _digiContext.Residents.Find(id);

            if ( dataResident == null)
            {
                TempData["message"] = "Data warga tidak ditemukan. Apakah data warga ada di dalam database?";
                return RedirectToAction("ErrorAction", "Home");
            }

            await _residentService.ResidentDelete(id);

            TempData["message"] = "Data warga berhasil di input";
            return RedirectToAction("Index");

        }

        public Guid GetCurrentUserGuid()
        {
            Guid userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            return userId;
        }

        public async Task<string> SaveFile(IFormFile dataFile)
        {
            var fileName = String.Empty;
            if (dataFile != null)
            {
                fileName = $"{Guid.NewGuid()}-{dataFile?.FileName}";
                string filePathName = _webHost.ContentRootPath + $"/images/{fileName}";

                using (var StreamWriter = System.IO.File.Create(filePathName))
                {
                    //await StreamWriter.WriteAsync(Common.StreamToBytes(request.GambarFile.OpenReadStream()));
                    await StreamWriter.WriteAsync(dataFile.OpenReadStream().ToBytes());
                }

                return $"images/{fileName}";
            }
            return String.Empty;
        }
    }
}
