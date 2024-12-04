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
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public UserController(IUserService userService, ITeamService teamService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _teamService = teamService;
            _httpContextAccessor = httpContextAccessor;
        }

        // Displays a list of active users
        public IActionResult Index(bool onlyAgents = false, string searchName = null, string searchEmail = null, int? searchRoleId = null)
        {
            // Retrieve users based on the onlyAgents flag
            var usersQuery = _userService.GetUser(onlyAgents = false);

            // Apply search filter by Name
            if (!string.IsNullOrEmpty(searchName))
            {
                // To perform a case-insensitive search, convert both the property and the search term to lower case
                usersQuery = usersQuery.Where(u => u.Name != null &&
                                                    u.Name.ToLower().Contains(searchName.ToLower()));
            }

            // Apply search filter by Email
            if (!string.IsNullOrEmpty(searchEmail))
            {
                usersQuery = usersQuery.Where(u => u.Email != null &&
                                                    u.Email.ToLower().Contains(searchEmail.ToLower()));
            }

            // Apply filter by RoleId
            if (searchRoleId.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.RoleId == searchRoleId.Value);
            }

            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var users = usersQuery.Where(user => user.IsActive && user.UserId != userId).ToList();

            // Pass the filtered user list to the view
            return View(users);
        }

        // Loads the Create User form
        public IActionResult Create()
        {
            ViewBag.Teams = _teamService.GetListOfTeams()
            .Select(team => new SelectListItem
            {
                Value = team.TeamId.ToString(),
                Text = team.TeamName
            })
            .ToList();

            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Super Admin", Value = "1" },
                new SelectListItem { Text = "Admin", Value = "2" },
                new SelectListItem { Text = "Agent", Value = "3" },
                new SelectListItem { Text = "User", Value = "4" },
            };

            var user = new User();
            return View(user); 
        }

        // CREATE
        [HttpPost]
        public IActionResult Create(User user) 
        {
            _userService.AddUser(user); 
            return RedirectToAction("Index","User");
        }
        public IActionResult Edit(string id)
        {
            var user = _userService.GetUsers().FirstOrDefault(x => x.UserId == id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index");
            }

            ViewBag.Teams = _teamService.GetListOfTeams()
            .Select(team => new SelectListItem
            {
                Value = team.TeamId.ToString(),
                Text = team.TeamName
            })
            .ToList();

            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Super Admin", Value = "1" },
                new SelectListItem { Text = "Admin", Value = "2" },
                new SelectListItem { Text = "Agent", Value = "3" },
                new SelectListItem { Text = "User", Value = "4" },
            };
            return View(user);
        }

        //EDIT USER
        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (user != null)
            {
                _userService.UpdateUser(user);
                TempData["SuccessMessage"] = "User has been edited";
            }

            return RedirectToAction("Index", "User");
        }

        // Marks a user as inactive instead of hard deleting them
        public IActionResult Delete(User user)
        {
            if (user != null)
            {
                _userService.DeleteUser(user);  // Perform soft delete
            }
            return RedirectToAction("Index","User");
        }

        [HttpGet]
        public IActionResult UserDetails(string id)
        {
            var user = _userService.GetUserDetails(id);

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public IActionResult UpdateProfile()
        {
            var user = _userService.GetMyProfile();

            var userModel = new UpdateUserViewModel
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.Name,
            };

            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index");
            }
            return View(userModel);
        }

        //EDIT USER
        [HttpPost]
        public IActionResult UpdateProfile(UpdateUserViewModel user)
        {
            if (user != null)
            {
                _userService.UpdateProfile(user);
                TempData["SuccessMessage"] = "User has been edited";
            }

            return RedirectToAction("Index", "UserTicket");
        }
    }
}
