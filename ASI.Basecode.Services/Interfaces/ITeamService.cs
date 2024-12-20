﻿using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public interface ITeamService
    {
        List<TeamViewModel> GetTeams();
        PagedTeamViewModel GetListOfTeams(string searchQuery, int page, int pageSize);
        TeamViewModel GetTeamById(int id);
        void AddTeam(TeamViewModel model);
        void EditTeam(TeamViewModel model);
        void DeleteTeam(int id);
        TeamDetailsDto? GetTeamDetails(int id, string? filter);
        void RemoveTeamMember(string userId);
        List<User> GetAdditionalMembers(int teamId);
        void AddAgentToTeam(string userId, int teamId);
    }
}
