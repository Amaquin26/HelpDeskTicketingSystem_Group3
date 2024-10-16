﻿using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;

        public TeamController(ITeamService teamService, IUserService userService)
        {
            _teamService = teamService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var teams = _teamService.GetListOfTeams();
            return View(teams);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.TeamLeaders = _userService.GetAgents().ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(TeamViewModel model)
        {
            if (ModelState.IsValid)
            {
                _teamService.AddTeam(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var team = _teamService.GetTeamById(id);
            ViewBag.TeamLeaders = _userService.GetAgents().ToList();
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        [HttpPost]
        public IActionResult Edit(TeamViewModel model)
        {
            if (ModelState.IsValid)
            {
                _teamService.EditTeam(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                _teamService.DeleteTeam(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

}
