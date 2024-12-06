using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Dto;
using LinqKit;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ASI.Basecode.WebApp.Controllers
{
    public class PreferenceController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PreferenceController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        // Displays a list of active users
        public IActionResult Index()
        {
            var preference = _userService.GetUserPreference();

            var preferenceViewModel = new PreferenceViewModel
            {
                ReceiveNotifications = preference.Item1,
                TicketViewMode = preference.Item1,
            };

            return View(preferenceViewModel);
        }

        [HttpPost]
        public IActionResult EditPreference(PreferenceViewModel preference)
        {
            _userService.EditUserPreference(preference.ReceiveNotifications,preference.TicketViewMode);

            return RedirectToAction("Index");
        }

    }
}
