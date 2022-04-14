using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class PostController : Controller
{
    private readonly ILogger<PostController> _logger;
    private readonly IPostService _postService;
    public PostController(
        ILogger<PostController> logger,    
        IPostService postService
    )
    {
        _logger = logger;
        _postService = postService;
    }

    public async Task<IActionResult> Index()
    {
        var dbResult = await _postService.GetAll();

        var viewModels = new List<PostModel>();

        for (int i = 0; i < dbResult.Count; i++)
        {
            viewModels.Add(new PostModel
            {
                PostId = dbResult[i].Id,
                PostSubject = dbResult[i].PostSubject,
                Title = dbResult[i].Title,
                Desc = dbResult[i].Desc,
                Image = dbResult[i].Image,
                Type = dbResult[i].Type,
                IsResidentProgram = dbResult[i].IsResidentProgram
            });
        }
        
        return View(viewModels);
    }

    public IActionResult Create()
    {
        return View(new PostModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PostModel request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }
        try
        {
            await _postService.Add(request.ConvertToDbModel());

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

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var dbResult = await _postService.Get(id);

        if (dbResult == null)
        {
            return NotFound();
        }

        return View(new PostModel(dbResult));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, PostModel request)
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
            await _postService.Update(request.ConvertToDbModel());
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid PostId)
    {
        if(PostId == null) {
            return BadRequest();
        }
        
        try{
            await _postService.Delete(PostId);

            return RedirectToAction(nameof(Index));  
        }catch(InvalidOperationException ex){
            ViewBag.ErrorMessage = ex.Message;
        }
        catch(Exception) {
            throw;
        }

        return View(PostId);
    }
}