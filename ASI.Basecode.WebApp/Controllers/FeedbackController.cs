﻿using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class FeedbackController : Controller
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    public IActionResult Index()
    {
        var feedbacks = _feedbackService.GetListOfFeedbacks();
        return View(feedbacks);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
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
    public JsonResult Delete(int id)
    {
        try
        {
            _feedbackService.DeleteFeedback(id);
            return Json(new { success = true });
        }
        catch (KeyNotFoundException)
        {
            return Json(new { success = false, message = "Feedback not found." });
        }
    }
}