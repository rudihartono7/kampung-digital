using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Trisatech.KampDigi.WebApp.Helpers;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class BaseController : Controller
{
    private IWebHostEnvironment _webHost;

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if(HttpContext.User == null || HttpContext.User.Identity == null){
            ViewBag.IsLogged = false;
        } else {
            ViewBag.IsLogged = HttpContext.User.Identity.IsAuthenticated;
        }

        base.OnActionExecuted(context);

    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _webHost = HttpContext.RequestServices.GetService<IWebHostEnvironment>();
        base.OnActionExecuting(context);
    }

    protected async Task<string> SaveFile(IFormFile dataFile)
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
