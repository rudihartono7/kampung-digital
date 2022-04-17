using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class CommentController : Controller
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentService _commentService;
    public CommentController(
        ILogger<CommentController> logger,    
        ICommentService commentService
    )
    {
        _logger = logger;
        _commentService = commentService;
    }

    public async Task<IActionResult> Index()
    {
        var dbResult = await _commentService.GetAll();

        var viewModels = new List<CommentModel>();

        for (int i = 0; i < dbResult.Count; i++)
        {
            viewModels.Add(new CommentModel
            {
                Id = dbResult[i].Id,
                PostId = dbResult[i].PostId,
                Desc = dbResult[i].Desc
            });
        }
        return View(viewModels);
    }
    public IActionResult Create()
    {
        return View(new CommentModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CommentModel request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }
        try
        {
            await _commentService.Add(request.ConvertToDbModel());

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
    // [HttpGet]
    // public async Task<IActionResult> Delete(Guid? id)
    // {
    //     if (id == null)
    //     {
    //         return BadRequest();
    //     }

    //     var result = await _commentService.Get(id.Value);

    //     if (result == null)
    //     {
    //         return NotFound();
    //     }

    //     return View(new CommentModel()
    //     {
    //         Id = result.Id,
    //         Desc = result.Desc,

    //     });
    // }
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
            await _commentService.Delete(id);

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