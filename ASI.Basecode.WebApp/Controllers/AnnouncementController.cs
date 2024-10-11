using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class AnnouncementController : Controller
{
    private readonly IAnnouncementService _announcementService;

    public AnnouncementController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    public IActionResult Index()
    {
        var announcements = _announcementService.GetListOfAnnouncements();
        return View(announcements);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(AnnouncementViewModel model)
    {
        if (ModelState.IsValid)
        {
            _announcementService.AddAnnouncement(model);
            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var announcement = _announcementService.GetListOfAnnouncements().FirstOrDefault(a => a.AnnouncementId == id);
        if (announcement == null)
        {
            return NotFound();
        }
        return View(announcement);
    }

    [HttpPost]
    public IActionResult Edit(AnnouncementViewModel model)
    {
        if (ModelState.IsValid)
        {
            _announcementService.EditAnnouncement(model);
            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpPost]
    public JsonResult Delete(int id)
    {
        try
        {
            _announcementService.DeleteAnnouncement(id);
            return Json(new { success = true });
        }
        catch (KeyNotFoundException)
        {
            return Json(new { success = false, message = "Announcement not found." });
        }
    }
}
