using System.Diagnostics;
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

public class AccountController : BaseController
{
    private readonly ILogger<ReportController> _logger;
    private readonly IAccountService _accountService;
    private readonly KampDigiContext _digiContext;

    public AccountController(ILogger<ReportController> logger,
    IAccountService accountService,
    KampDigiContext kampDigiContext)
    {
        _logger = logger;
        _accountService = accountService;
        _digiContext = kampDigiContext;
    }

    [Authorize(Roles = AppConstant.ADMIN)]
    public async Task<IActionResult> Index()
    {
        if (TempData["message"] != null)
        {
            ViewBag.Message = TempData["message"].ToString();
            TempData.Remove("message");
        }

        var listUser = await _accountService.GetAllUser();
        return View(listUser);
    }
    
    public async Task<IActionResult> Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterAdminModel dataUser)
    {
        if (!ModelState.IsValid)
        {
            return View(dataUser);
        }
        try
        {
            await _accountService.Register(dataUser);

            return Redirect(nameof(Login));
        }
        catch (InvalidOperationException ex)
        {
            ViewBag.ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            throw;
        }

        return View(dataUser);
    }

    public IActionResult Login()
    {
        
        return View(new LoginModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel dataInput)
    {
        var result = await _accountService.Login(dataInput.Username, dataInput.Password);
        string roleUser = string.Empty;

        if (result.Role == (Role)0)
        {
            roleUser = AppConstant.ADMIN;
        }
        else if (result.Role == (Role)1)
        {
            roleUser = AppConstant.HEADOFRESIDENCE;
        }
        else if (result.Role == (Role)2)
        {
            roleUser = AppConstant.HEADOFPROGRAM;
        }
        else
        {
            roleUser = AppConstant.RESIDENT;
        }
        try
        {

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()),
                    new Claim(ClaimTypes.Name, result.Name ?? result.Username),
                    new Claim(ClaimTypes.Role, roleUser),
                    new Claim(ClaimTypes.UserData, result.ResidentId.ToString())
                };

            var claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {

            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

            if (result.Role != (Role)0)
            {
                return RedirectToActionPermanent("ResidentDetail", "Resident", new { id = result.ResidentId });
            }
            return RedirectToActionPermanent("Index", "Home");
        }
        catch (System.Exception)
        {
            return View(dataInput);
        }
    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = AppConstant.ADMIN)]
    public async Task<IActionResult> EditRole(Guid id)
    {
        if (TempData["message"] != null)
        {
            ViewBag.Message = TempData["message"].ToString();
            TempData.Remove("message");
        }

        var dataUser = await _digiContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (dataUser == null)
        {
            TempData["message"] = "User tidak dapat ditemukan";
            return View();
        }

        return View(new EditRoleModel
        {
            Id = id,
            Username = dataUser.Username,
            Role = dataUser.Role,
        });
    }

    [Authorize(Roles = AppConstant.ADMIN)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRole(EditRoleModel dataRole)
    {
        if (!ModelState.IsValid)
        {
            TempData["message"] = "Model Tidak Valid";
            return View(dataRole);
        }

        await _accountService.EditRole(dataRole, GetCurrentUserGuid());

        TempData["message"] = "Role berhasil diupdate";
        return RedirectToAction("Index");

    }

    [Authorize]
    public async Task<IActionResult> UpdatePassword()
    {
        
        return View(new UpdatePasswordModel
        {
            Id = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value)
        });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordModel dataPassword)
    {
        if (!ModelState.IsValid)
        {
            return View(dataPassword);
        }

        var updatePassword = await _accountService.UpdatePassword(dataPassword);
        TempData["message"] = "Password berhasil diupdate";
        return RedirectToAction("ResidentDetail", "Resident", new { id = dataPassword.Id });
    }

    public async Task<IActionResult> ResetPassword(Guid id)
    {

        return View(new ResetPasswordModel
        {
            Id = id,
        });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel dataPassword)
    {
        if (!ModelState.IsValid)
        {
            return View(dataPassword);
        }

        var updatePassword = await _accountService.ResetPassword(dataPassword, GetCurrentUserGuid());
        

        TempData["message"] = "Password berhasil direset";
        return RedirectToAction("Index", "Account");
    }

    //public async Task<IActionResult> DeleteUser (Guid id)
    //{
    //    var dataUser = await _digiContext.Users.FindAsync(id);

    //    if ( dataUser == null)
    //    {
    //        TempData["message"] = "Data users/akun tidak ditemukan";
    //        return RedirectToAction("ErrorAction", "Home");
    //    }

    //    await _accountService.DeleteUser(dataUser);
    //    TempData["message"] = "Akun berhasil dihapus";
    //    return RedirectToAction("Index", "Account");
    //}

    public Guid GetCurrentUserGuid()
    {
        Guid userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        return userId;
    }
}
