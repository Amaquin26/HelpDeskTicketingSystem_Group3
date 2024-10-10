using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        // Displays a list of active users
        public IActionResult Index(bool onlyAgents = false)
        {
            List<User> users = _userService.GetUser(onlyAgents)
                .Select(user => new User
                {
                    UserId = user.UserId,
                    Name = user.Name ?? "Unknown", // Provide a default value if null
                    Email = user.Email ?? "No Email", // Provide a default value if null
                    RoleId = user.RoleId,
                    IsActive = user.IsActive
                })
                .ToList();

            return View(users); // Pass the list of users to the view
        }

        // Loads the Create User form
        public IActionResult Create()
        {
            return View();
        }

        // Handles the form submission for creating a new user
        [HttpPost]
        public IActionResult Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                _userService.AddUser(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // Loads the Edit User form for the specified user ID
        public IActionResult Edit(string id)
        {
            var user = _userService.GetUsers().FirstOrDefault(u => u.UserId == id);
            return View(user);
        }

        // Handles the form submission for editing an existing user
        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (user != null)
            {
                _userService.UpdateUser(user);
            }
            return RedirectToAction("Index");
        }

        // Marks a user as inactive instead of hard deleting them
        public IActionResult Delete(string id)
        {
            var user = _userService.GetUsers().FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                _userService.DeleteUser(user);
            }
            return RedirectToAction("Index");
        }
    }
}
