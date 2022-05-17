using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.WebApp.Helpers;
using Trisatech.KampDigi.WebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Trisatech.KampDigi.Application;


namespace Trisatech.KampDigi.WebApp.Controllers;

public class PublicationController : BaseController
{
    private readonly ILogger<PublicationController> _logger;
    private readonly IPublicationService _publicationService;
    private readonly IWebHostEnvironment _iWebHost;
    public PublicationController(ILogger<PublicationController> logger,
    IPublicationService publicationService, IWebHostEnvironment iWebHost)
    {
        _logger = logger;
        _publicationService = publicationService;
        _iWebHost = iWebHost;
    }

    [Authorize(Roles = AppConstant.ADMIN)]
    public async Task<IActionResult> Index()
    {
        if (TempData["message"] != null)
        {
            ViewBag.Message = TempData["message"].ToString();
            TempData.Remove("message");
        }

        var dbResult = await _publicationService.GetAll();

        var models = new List<PublicationModel>();

        for (int i = 0; i < dbResult.Count; i++)
        {
            models.Add(new PublicationModel{
                Id = dbResult[i].Id,
                Title = dbResult[i].Title,
                ImageLink = dbResult[i].ImageLink,
                PublishDate = dbResult[i].PublishDate,
                Writer = dbResult[i].Writer,
                Content = dbResult[i].Content,
                Source = dbResult[i].Source,
            });
        }

        return View(models);
    }

    [Authorize]
    public async Task<IActionResult> ListPublication()
    {
        var dbResult = await _publicationService.GetAll();

        var models = new List<PublicationModel>();

        for (int i = 0; i < dbResult.Count; i++)
        {
            models.Add(new PublicationModel{
                Id = dbResult[i].Id,
                Title = dbResult[i].Title,
                ImageLink = dbResult[i].ImageLink,
                PublishDate = dbResult[i].PublishDate,
                Writer = dbResult[i].Writer,
                Content = dbResult[i].Content,
                Source = dbResult[i].Source,
            });
        }

        return View(models);
    }

    [Authorize(Roles = AppConstant.ADMIN)]
    public IActionResult Create() {
        return View(new PublicationModel());
    }

    [Authorize(Roles = AppConstant.ADMIN)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PublicationModel request) {
        if(!ModelState.IsValid){
            return View(request);
        }
        try{
            string fileName = string.Empty;
            
            if(request.ImageFile != null) 
            {
                fileName = $"{request.ImageFile?.FileName}";

                string filePathName = _iWebHost.WebRootPath + "/publicationImage/" +fileName;
                
                using(var streamWriter = System.IO.File.Create(filePathName)){
                    //await streamWriter.WriteAsync(Common.StreamToBytes(request.GambarFile.OpenReadStream()));
                    //using extension to convert stream to bytes
                    await streamWriter.WriteAsync(request.ImageFile.OpenReadStream().ToBytes());
                }
            }

            request.ImageLink = $"publicationImage/{fileName}";

            await _publicationService.Add(request.ConvertToDbModel());

            return Redirect(nameof(Index));   
        }catch(InvalidOperationException ex){
            ViewBag.ErrorMessage = ex.Message;
        }
        catch(Exception) {
            throw;
        }

        return View(request);
    }

    [Authorize]
    public async Task<IActionResult> Details(Guid? id){
        if(id == null)
        {
            return BadRequest();
        }

        var result = await _publicationService.Get(id.Value);

        if(result == null) {
            return NotFound();
        }

        return View(new PublicationModel(result));
    }

    [Authorize(Roles = AppConstant.ADMIN)]
    [HttpGet]
    public async Task<IActionResult> Edit(Guid? id)
     {
          if (id == null)
          {
               return new NotFoundResult();
          }

          var data = await _publicationService.Get(id.Value);
          if (data == null)
          {
               return NotFound();
          }

          return View(new PublicationModel()
          {
               Id = data.Id,
               Title = data.Title,
               Slag = data.Slag,
               PublishDate = data.PublishDate,
               ImageLink = data.ImageLink,
               Content = data.Content,
               Writer = data.Writer,
               Source = data.Source,
          });
     }

        [Authorize(Roles = AppConstant.ADMIN)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPublication(Guid? id, PublicationModel request)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }


            try{
                string fileName = string.Empty;
            
            if(request.ImageFile != null) 
            {
                fileName = $"{request.ImageFile?.FileName}";

                string filePathName = _iWebHost.WebRootPath + "/publicationImage/" +fileName;
                
                using(var streamWriter = System.IO.File.Create(filePathName)){
                    //await streamWriter.WriteAsync(Common.StreamToBytes(request.GambarFile.OpenReadStream()));
                    //using extension to convert stream to bytes
                    await streamWriter.WriteAsync(request.ImageFile.OpenReadStream().ToBytes());
                }
            }
            
            var publication = request.ConvertToDbModel();
            if (request.ImageFile ==null){
                publication.ImageLink = request.ImageLink;
            }
            if(request.ImageFile != null) 
            {
                publication.ImageLink =$"publicationImage/{fileName}";
            }
                        
                await _publicationService.Update(publication);
                return RedirectToAction(nameof(Index));  
            }
            catch(InvalidOperationException ex){
                ViewBag.ErrorMessage = ex.Message;
            }
            catch(Exception) {
                throw;
            }   
                return View(request);
        }

        [Authorize(Roles = AppConstant.ADMIN)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try{
                var delete = await _publicationService.Delete(id);
                if (delete){
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception){
            throw;
            }
            return View(new PublicationModel());
        }
    


}
