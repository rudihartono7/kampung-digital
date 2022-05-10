using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;

namespace Trisatech.KampDigi.WebApp.Controllers;

public class CommentController : BaseController
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentService _commentService;
    private readonly IPostService _postService; 
    public CommentController(
        ILogger<CommentController> logger,
        ICommentService commentService,
        IPostService postService
    )
    {
        _logger = logger;
        _commentService = commentService;
        _postService = postService;
    }

    // public async Task<IActionResult> Index(){
    //     var idPost = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
    //     var result = await _commentService.Get(Guid.Parse(idPost));

    //     return View(result);
    // }

    [Authorize]
    public async Task<IActionResult> Index(Guid id)
    {
        var dbResult = await _commentService.GetComments(id);

        var Desc = await _postService.Get(id);
        var PostSubject = await _postService.Get(id);
        var CreatedDate = await _postService.Get(id);
        var UpdatedDate = await _postService.Get(id);
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

        ViewBag.Desc = Desc.Desc;
        ViewBag.PostSubject = PostSubject.PostSubject;
        ViewBag.CreatedDate = CreatedDate.CreatedDate;
        ViewBag.UpdatedDate = UpdatedDate.UpdatedDate;
        ViewBag.Thread = id;
        return View(viewModels);
    }
    [Authorize]
    public async Task<IActionResult> Create(Guid id)
    {

        return View(new CommentModel
        {
            Desc = "",
            PostId = id
        });
    }


    [Authorize]
    [HttpPost]
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PostId, Desc")] Guid PostId, string Desc, CommentModel request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            await _commentService.Add(request.ConvertToDbModelCreate());

            return RedirectToAction("Index", new { Id = request.PostId });
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
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var dbResult = await _commentService.Get(id.Value);

        if (dbResult == null)
        {
            return NotFound();
        }

        return View(new CommentModel()
        {
            Id = dbResult.PostId,
            Desc = dbResult.Desc,
            PostId = dbResult.PostId

        });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, CommentModel request)
    {
        if (id == null)
        {
            return BadRequest();
        }

        try
        {
            await _commentService.Delete(id);

            return RedirectToAction("Index", new { id = request.PostId });
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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var comment = await _commentService.Get(id);

        if (comment == null)
        {
            return NotFound();
        }
        return View(new CommentModel()
        {
            Id = id,
            Desc = comment.Desc,
            PostId = comment.PostId
        });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CommentModel request)
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
            await _commentService.Update(request.ConvertToDbModelEdit());
            return RedirectToAction("Index", new { id = request.PostId });
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
}