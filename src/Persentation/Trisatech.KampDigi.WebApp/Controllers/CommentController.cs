using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class CommentController : BaseController
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

    // public async Task<IActionResult> Index(){
    //     var idPost = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
    //     var result = await _commentService.Get(Guid.Parse(idPost));

    //     return View(result);
    // }

    public async Task<IActionResult> Index(Guid id)
    {
        var dbResult = await _commentService.GetComments(id);

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
    public async Task<IActionResult> Create(Guid id)
    {

        return View(new CommentModel
        {
            Desc = "",
            PostId = id
        });
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Guid id, CommentModel request)
    {

        
        try
        {
            await _commentService.Add(request.ConvertToDbModelCreate());

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
    //         PostId = result.PostId

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