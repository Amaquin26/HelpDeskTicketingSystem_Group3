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
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public ProfileController(IUserService userService, ITeamService teamService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _teamService = teamService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index(string filter = null)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userService.GetUserDetails(userId, filter);   

            return View(user);
        }
    }
}
