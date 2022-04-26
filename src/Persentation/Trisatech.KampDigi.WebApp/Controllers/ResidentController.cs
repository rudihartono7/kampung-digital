using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Models.Resident;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Domain;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Trisatech.KampDigi.Application;
using Trisatech.KampDigi.Application.Models;

namespace Trisatech.KampDigi.WebApp.Controllers
{
    public class ResidentController : BaseController
    {
        private readonly IResidentService _residentService;
        private readonly KampDigiContext _digiContext;
        private readonly IGuestBookService _guestBookService;
        public ResidentController(IResidentService residentService,
            KampDigiContext digiContext,
            IGuestBookService guestBookService)
        {
            _residentService = residentService;
            _digiContext = digiContext;
            _guestBookService = guestBookService;
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
                ViewBag.Message = "Data input tidak valid. Pastikan data sudah terisi lengkap.";
                ViewBag.House = new SelectList(_digiContext.Houses, "Id", "Number");
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
                ViewBag.Message = ex.Message;
                ViewBag.House = new SelectList(_digiContext.Houses, "Id", "Number");
                return View(dataResident);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        public async Task<IActionResult> ResidentDetail(Guid id)
        {
            if (_digiContext.Residents.Find(id) == null)
            {
                TempData["message"] = "Data warga tidak ditemukan. Apakah data warga sudah di input?";
                return RedirectToAction("ErrorAction", "Home");
            }

            var residentDetail = await _residentService.ResidentDetail(id);
            var guestList = await _guestBookService.GuestResidentList(id);

            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"].ToString();
                TempData.Remove("message");
            }

            ViewBag.House = new SelectList(_digiContext.Houses, "Id", "Number");

            return View(new UserDetailModel
            {
                Residents = residentDetail,
                Guests = guestList,
                GuestEdit = new Application.Models.GuestBook.GuestBookListModel(),
            });
        }

        public async Task<IActionResult> ResidentEdit(Guid id)
        {
            var residentDetail = await _residentService.ResidentGetEditModel(id);
            if (residentDetail == null)
            {
                TempData["message"] = "Data warga tidak ditemukan. Apakah data warga sudah di input?";
                return RedirectToAction("ErrorAction", "Home");
            }


            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"].ToString();
                TempData.Remove("message");
            }

            ViewBag.House = new SelectList(_digiContext.Houses, "Id", "Number");

            return View(residentDetail);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResidentEdit(ResidentEditModel dataResident)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Data input tidak valid. Pastikan data sudah terisi lengkap.";
                ViewBag.House = new SelectList(_digiContext.Houses, "Id", "Number");
                return View(dataResident);
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

                TempData["message"] = "Data berhasil di ubah.";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                TempData["message"] = ex.Message;
                return RedirectToAction("ErrorAction", "Home");
            }
            catch (Exception)
            {
                throw;
            }
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

            try
            {
                await _residentService.ResidentDelete(id);

                TempData["message"] = "Data warga berhasil di hapus";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Message = ex.Message;
                return View(dataResident);
            }

        }

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
