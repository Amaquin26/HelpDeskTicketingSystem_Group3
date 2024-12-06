using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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

        public IActionResult Index(string searchQuery, int page = 1, int pageSize = 2)
        {
            var pagedTeamViewModel = _teamService.GetListOfTeams(searchQuery, page, pageSize);

            // Build the view model
            var viewModel = new TeamListViewModel
            {
                Teams = pagedTeamViewModel.teams,
                CurrentPage = page,
                TotalPages = pagedTeamViewModel.totalPages,
                SearchQuery = searchQuery
            };

            return View(viewModel);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.TeamLeaders = _userService.GetAgents()
            .Select(teamLeader => new SelectListItem
            {
                Value = teamLeader.UserId,
                Text = teamLeader.Name
            })
            .ToList();

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
            ViewBag.TeamLeaders = _userService.GetAgents()
            .Select(teamLeader => new SelectListItem
            {
                Value = teamLeader.UserId,
                Text = teamLeader.Name
            })
            .ToList();
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
        public IActionResult Delete(int teamId)
        {
            _teamService.DeleteTeam(teamId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult TeamDetails(int id, string filter = null)
        {
            var team = _teamService.GetTeamDetails(id, filter);

            if (team == null)
            {
                return RedirectToAction("Index");
            }

            return View(team);
        }

        [HttpPost]
        public IActionResult RemoveTeamMember(string userId, int teamId)
        {
            _teamService.RemoveTeamMember(userId);
            return RedirectToAction("TeamDetails", new { Id = teamId });
        }

        [HttpGet]
        public IActionResult AddMember(int teamId)
        {
            var agents = _teamService.GetAdditionalMembers(teamId);

            var agentItems = new List<SelectListItem>();

            foreach (User agent in agents)
            {
                var listItem = new SelectListItem { Text = agent.Name, Value = agent.UserId };
                agentItems.Add(listItem);
            }

            ViewBag.Agents = agentItems;
            ViewBag.TeamId = teamId;

            return View();
        }

        [HttpPost]
        public IActionResult AddMember(AddTeamMemberViewModel agent)
        {
            List<User> agents;
            List<SelectListItem> agentItems;


            if (!ModelState.IsValid)
            {
                agents = _teamService.GetAdditionalMembers(agent.TeamId);

                agentItems = new List<SelectListItem>();

                foreach (User a in agents)
                {
                    var listItem = new SelectListItem { Text = a.Name, Value = a.UserId };
                    agentItems.Add(listItem);
                }

                ViewBag.Agents = agentItems;
                ViewBag.TeamId = agent.TeamId;

                return View();
            }

            _teamService.AddAgentToTeam(agent.UserId, agent.TeamId);

            return RedirectToAction("TeamDetails", new { Id = agent.TeamId });
        }
    }
}
