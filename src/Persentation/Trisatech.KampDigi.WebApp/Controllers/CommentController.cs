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
}