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
        private readonly IGuestBookService _guestBookService;
        private readonly KampDigiContext _digiContext;
        public GuestBookController(IGuestBookService guestBookService,
            KampDigiContext digiContext)
        {
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
                TempData["message"] = ex.Message;
                return RedirectToAction("ErrorAction", "Home");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuestEdit(GuestBookEditModel dataGuest)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Data input tidak valid. Pastikan data sudah terisi lengkap.";
                return RedirectToAction("ResidentDetail", "Resident", new { id = GetCurrentUserGuid() });
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
                    return RedirectToAction("ResidentDetail", "Resident", new { id = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData).Value) });
                }

            }
            catch (InvalidOperationException ex)
            {
                TempData["message"] = ex.Message;
                return RedirectToAction("ErrorAction", "Home");
            }
        }

        public Guid GetCurrentUserGuid()
        {
            Guid userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            return userId;
        }
    }

}

