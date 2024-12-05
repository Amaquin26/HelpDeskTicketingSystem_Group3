using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

public class FeedbackController : Controller
{
    private readonly IFeedbackService _feedbackService;
    private readonly ITicketService _ticketService;

    public FeedbackController(IFeedbackService feedbackService, ITicketService ticketService)
    {
        _feedbackService = feedbackService;
        _ticketService = ticketService;
    }

    public IActionResult Index(string searchQuery, int page = 1, int pageSize = 5)
    {
        var feedbacks = _feedbackService.GetListOfFeedbacks();

        // Apply search filter
        if (!string.IsNullOrEmpty(searchQuery))
        {
            feedbacks = feedbacks.Where(f => f.CreatedBy.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                                             f.Comment.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var pagedFeedbacks = feedbacks.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var totalPages = (int)Math.Ceiling(feedbacks.Count / (double)pageSize);

        var viewModel = new FeedbackListViewModel
        {
            Feedbacks = pagedFeedbacks,
            TotalPages = totalPages,
            CurrentPage = page,
            SearchQuery = searchQuery
        };

        return View(viewModel);
    }


    [HttpGet]
    public IActionResult Create()
    {
        var feedbackViewModel = new FeedbackViewModel();

        feedbackViewModel.Tickets = _ticketService.GetListOfTickets().Select(t => new Ticket
        {
            TicketId = t.TicketId,
            Title = t.Title,
            Description = t.Description,
        }).ToList();

        return View(feedbackViewModel);
    }

    [HttpPost]
    public IActionResult Create(FeedbackViewModel model)
    {
        if (ModelState.IsValid)
        {
            _feedbackService.AddFeedback(model);
            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var feedback = _feedbackService.GetListOfFeedbacks().FirstOrDefault(f => f.FeedbackId == id);
        if (feedback == null)
        {
            return NotFound();
        }
        return View(feedback);
    }

    [HttpPost]
    public IActionResult Edit(FeedbackViewModel model)
    {
        if (ModelState.IsValid)
        {
            _feedbackService.EditFeedback(model);
            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpPost]
    public IActionResult Delete(int feedbackId)
    {
        _feedbackService.DeleteFeedback(feedbackId);
        return RedirectToAction("Index");
    }
}
