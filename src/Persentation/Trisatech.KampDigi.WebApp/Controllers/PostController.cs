using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Trisatech.KampDigi.WebApp.Helpers;
using Trisatech.KampDigi.Application;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class PostController : BaseController
{
    private readonly ILogger<PostController> _logger;
    private readonly IPostService _postService;
    private readonly IWebHostEnvironment _iwebHost;
    public PostController(
        ILogger<PostController> logger,
        IPostService postService,
        IWebHostEnvironment iwebHost
    )
    {
        _logger = logger;
        _postService = postService;
        _iwebHost = iwebHost;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var dbResult = await _postService.GetAll();

        var viewModels = new List<PostModel>();

        for (int i = 0; i < dbResult.Count; i++)
        {
            viewModels.Add(new PostModel
            {
                Id = dbResult[i].Id,
                PostSubject = dbResult[i].PostSubject,
                Title = dbResult[i].Title,
                Desc = dbResult[i].Desc,
                Image = dbResult[i].Image,
                Type = dbResult[i].Type,
                IsResidentProgram = dbResult[i].IsResidentProgram,
                CreatedDate = dbResult[i].CreatedDate,
                UpdatedDate = dbResult[i].UpdatedDate
            });
        }

        return View(viewModels);
    }

    [Authorize]
    public async Task<IActionResult> Create()
    {
        return View(new PostModel
        {
            PostSubject = "",
            Title = "",
            Desc = "",
            Image = "",
            IsResidentProgram = false
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(PostModel request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }
        try
        {
            string fileName = string.Empty;

            if (request.ImageFile != null)
            {
                fileName = $"{Guid.NewGuid()}-{request.ImageFile?.FileName}";
                string filePathName = _iwebHost.WebRootPath + $"\\images\\{fileName}";

                using (var streamWriter = System.IO.File.Create(filePathName))
                {
                    await streamWriter.WriteAsync(request.ImageFile.OpenReadStream().ToBytes());
                }
            }
            
            var post = request.ConvertToDbModelAdd();
            post.Image = $"\\images\\{fileName}";
            
            await _postService.Add(post);
            return Redirect(nameof(Index));
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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var post = await _postService.Get(id);

        if (post == null)
        {
            return NotFound();
        }
        return View(new PostModel()
        {
            Id = id,
            PostSubject = post.PostSubject,
            Title = post.Title,
            Desc = post.Desc,
            Image = post.Image,
            Type = post.Type,
            IsResidentProgram = post.IsResidentProgram
        });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, PostModel request)
    {
        if (id == null)
        {
            return BadRequest();
        }
        if (!ModelState.IsValid)
        {
            return View(request);
        }
        try
        {
            string fileName = string.Empty;
            if (request.ImageFile != null)
            {
                fileName = $"{Guid.NewGuid()}-{request.ImageFile?.FileName}";
                string filePathName = _iwebHost.WebRootPath + "\\images\\" + fileName;

                using (var streamWriter = System.IO.File.Create(filePathName))
                {
                    await streamWriter.WriteAsync(request.ImageFile.OpenReadStream().ToBytes());
                }
            }
            var produk = request.ConvertToDbModelEdit();
            if (request.ImageFile != null)
            {
                produk.Image = $"/images/{fileName}";
            }
            await _postService.Update(request.ConvertToDbModelEdit());
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {

            ViewBag.ErrorMessage = ex.Message;
        }
        catch
        {
            throw;
        }
        return View(request);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var result = await _postService.Get(id.Value);

        if (result == null)
        {
            return NotFound();
        }

        return View(new PostModel()
        {
            Id = result.Id,
            PostSubject = result.PostSubject,
            Title = result.Title,
            Desc = result.Desc,
            Image = result.Image,
            Type = result.Type,
            IsResidentProgram = result.IsResidentProgram
        });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        try
        {
            await _postService.Delete(id);

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

        return View(id);
    }
}