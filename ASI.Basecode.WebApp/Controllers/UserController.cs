using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Index(bool onlyAgents = false, string searchName = null, string searchEmail = null, int? searchRoleId = null)
        {
            // Retrieve users based on the onlyAgents flag
            var usersQuery = _userService.GetUser(onlyAgents).Where(u => u.IsActive);

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

            // Execute the query and convert it to a list
            var users = usersQuery.ToList();

            // Define roles for the dropdown
            ViewBag.Roles = new List<SelectListItem>
    {
        new SelectListItem { Value = "1", Text = "Super Admin" },
        new SelectListItem { Value = "2", Text = "Admin" },
        new SelectListItem { Value = "3", Text = "Agents" },
        new SelectListItem { Value = "4", Text = "User" }
    };

            // Pass the filtered user list to the view
            return View(users);
        }



        // Loads the Create User form
        public IActionResult Create()
        {
            return View(); 
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
                return RedirectToAction("Index");// Optionally handle the case where the user is not found
            }
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
    }
}
