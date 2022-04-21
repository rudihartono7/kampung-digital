using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Trisatech.KampDigi.Application;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models.GuestBook;
using Trisatech.KampDigi.Application.Models.Resident;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.WebApp.Helpers;

namespace Trisatech.KampDigi.WebApp.Controllers
{
    public class GuestBookController : BaseController
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly IGuestBookService _guestBookService;
        private readonly KampDigiContext _digiContext;
        public GuestBookController(IWebHostEnvironment webHost,
            IGuestBookService guestBookService,
            KampDigiContext digiContext)
        {
            _webHost = webHost;
            _guestBookService = guestBookService;
            _digiContext = digiContext;
        }

        [Authorize(Roles = AppConstant.ADMIN)]
        public async Task<IActionResult> Index()
        {
            var listGuest = await _guestBookService.GuestBookList();

            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"].ToString();
                TempData.Remove("message");
            }

            ViewBag.Resident = new SelectList(_digiContext.Residents, "Id", "Name");
            return View(listGuest);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuestAdd(GuestBookAddModel dataTamu)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Data input tidak valid. Pastikan data sudah terisi lengkap.";
                return View(dataTamu);
            }
            try
            {
                dataTamu.ImageUrl = await SaveFile(dataTamu.PhotoTamu);


                await _guestBookService.GuestAdd(dataTamu, GetCurrentUserGuid());

                if (User.IsInRole(AppConstant.ADMIN))
                {
                    TempData["message"] = "Data tamu berhasil ditambahkan";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "Data tamu berhasil ditambahkan";
                    return RedirectToAction("ResidentDetail", "Resident", new { id = dataTamu.GuestToId });
                }
                
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            catch (Exception)
            {
                throw;
            }

            return View(dataTamu);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuestEdit(GuestBookEditModel dataGuest)
        {
            var dataResident = _digiContext.Users.FirstOrDefault(x => x.Id == GetCurrentUserGuid());

            if (!ModelState.IsValid)
            {
                TempData["message"] = "Data input tidak valid. Pastikan data sudah terisi lengkap.";
                return RedirectToAction("ResidentDetail", "Resident", new { id = dataResident.ResidentId });
            }
            try
            {
                await _guestBookService.GuestBookEdit(dataGuest, GetCurrentUserGuid());

                if (User.IsInRole(AppConstant.ADMIN))
                {
                    TempData["message"] = "Data tamu berhasil diedit";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "Data tamu berhasil diedit";
                    return RedirectToAction("ResidentDetail", "Resident", new { id = dataResident.ResidentId });
                }

            }
            catch (InvalidOperationException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            catch (Exception)
            {
                throw;
            }

            return View(dataGuest);
        }

        //[Authorize]
        //public async Task<IActionResult> GuestDetail(Guid id)
        //{
        //    if (_digiContext.GuestBooks.Find(id) == null)
        //    {
        //        TempData["message"] = "Data tamu tidak ditemukan. Apakah data tamu sudah di input?";
        //        return RedirectToAction("ErrorAction", "Home");
        //    }


        //    if (TempData["message"] != null)
        //    {
        //        ViewBag.Message = TempData["message"].ToString();
        //        TempData.Remove("message");
        //    }
        //    return View(await _guestBookService.GuestDetail(id));
        //}


        public Guid GetCurrentUserGuid()
        {
            Guid userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            return userId;
        }

        //public async Task<string> SaveFile(IFormFile dataFile)
        //{
        //    var fileName = String.Empty;
        //    if (dataFile != null)
        //    {
        //        fileName = $"{Guid.NewGuid()}-{dataFile?.FileName}";
        //        string filePathName = _webHost.ContentRootPath + $"/images/{fileName}";

        //        using (var StreamWriter = System.IO.File.Create(filePathName))
        //        {
        //            //await StreamWriter.WriteAsync(Common.StreamToBytes(request.GambarFile.OpenReadStream()));
        //            await StreamWriter.WriteAsync(dataFile.OpenReadStream().ToBytes());
        //        }

        //        return $"images/{fileName}";
        //    }
        //    return String.Empty;
        //}
    }

}

