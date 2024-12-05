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
        public IActionResult Index(string searchQuery, int page = 1, int pageSize = 5)
        {
            // Retrieve users based on the onlyAgents flag
            var users = _userService.GetUsers();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                users = users.Where(u => u.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                u.Role.RoleName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                u.Email.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            users = users.Where(user => user.UserId != userId).ToList();

            var pagedUsers = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var totalPages = (int)Math.Ceiling(users.Count / (double)pageSize);

            var viewModel = new UserListViewModel
            {
                Users = pagedUsers,
                TotalPages = totalPages,
                CurrentPage = page,
                SearchQuery = searchQuery,
            };

            // Pass the filtered user list to the view
            return View(viewModel);
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

            var roles = _userService.GetUserRoles();

            var roleItems = new List<SelectListItem>();

            foreach(Role role in roles)
            {
                var listItem = new SelectListItem { Text = role.RoleName, Value = role.RoleId.ToString()};
                roleItems.Add(listItem);
            }

            ViewBag.Roles = roleItems;

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

        [HttpGet]
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

            var roles = _userService.GetUserRoles();

            var roleItems = new List<SelectListItem>();

            foreach (Role role in roles)
            {
                var listItem = new SelectListItem { Text = role.RoleName, Value = role.RoleId.ToString() };
                roleItems.Add(listItem);
            }

            ViewBag.Roles = roleItems;

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
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                _userService.UpdateProfile(user);
                TempData["SuccessMessage"] = "User has been edited";
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            var role = _httpContextAccessor.HttpContext?.User.FindFirst("Role")?.Value;

            if (role == "User")
            {
                return RedirectToAction("Index", "UserTicket");
            }
            else
            {
                return RedirectToAction("Index", "Profile");
            }
        }
    }
}
