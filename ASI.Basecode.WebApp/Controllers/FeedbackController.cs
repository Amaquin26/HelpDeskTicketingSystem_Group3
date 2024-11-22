using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Mvc;
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

    public IActionResult Index()
    {
        var feedbacks = _feedbackService.GetListOfFeedbacks();
        return View(feedbacks);
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
