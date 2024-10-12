using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public LoginResult AuthenticateUser(string email, string password, ref User user)
        {
            user = new User();
            var passwordKey = PasswordManager.EncryptPassword(password);
            user = _repository.GetUsers().Where(x => x.Email == email &&
                                                     x.Password == passwordKey).FirstOrDefault();

            return user != null ? LoginResult.Success : LoginResult.Failed;
        }

        public void AddUser(UserViewModel model)
        {
            var user = new User();
            if (!_repository.EmailExists(model.Email))
            {
                _mapper.Map(model, user);
                user.Password = PasswordManager.EncryptPassword(model.Password);
                user.CreatedTime = DateTime.Now;
                user.UpdatedTime = DateTime.Now;
                user.CreatedBy = System.Environment.UserName;
                user.UpdatedBy = System.Environment.UserName;

                Guid userId = Guid.NewGuid();
                user.UserId = userId.ToString();
                user.RoleId = 1;
                user.Name = model.Name;
                user.IsActive = model.IsActive;
                user.Email = "Jermain@buang.com";
                _repository.AddUser(user);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserExists);
            }
        }
        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!_repository.UserExists(user.Email)) // Check if user exists by email
            {
                // Set additional properties for the new user
                user.UserId = Guid.NewGuid().ToString(); 
                user.Password = PasswordManager.EncryptPassword(user.Password);
                user.CreatedTime = DateTime.Now;
                user.UpdatedTime = DateTime.Now;
                user.CreatedBy = Environment.UserName;
                user.UpdatedBy = Environment.UserName;

                _repository.AddUser(user);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserExists);
            }
        }
        public List<User> GetUsers()
        {
            return _repository.GetUsers().Where(u => !u.IsActive).ToList();
        }
        public void DeleteUser(User user)
        {
            var existingUser = _repository.GetUsers().FirstOrDefault(u => u.UserId == user.UserId);
            if (existingUser != null)
            {
                existingUser.IsActive = user.IsActive == false;
                _repository.DeleteUser(existingUser);
            }
        }
        public void UpdateUser(User user)
        {
            var existingUser = _repository.GetUsers().FirstOrDefault(u => u.UserId == user.UserId);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                if (!string.IsNullOrEmpty(user.Password))
                {
                    existingUser.Password = PasswordManager.EncryptPassword(user.Password); // Optional: Encrypt password if necessary
                }
                existingUser.RoleId = user.RoleId;
                existingUser.UpdatedTime = DateTime.Now;
                existingUser.UpdatedBy = Environment.UserName;

                _repository.UpdateUser(existingUser);
            }
        }

        public IQueryable<User> GetUser(bool onlyAgents)
        {
            if (onlyAgents)
            {
                return _repository.GetAgents(); // Return only agents
            }
            return _repository.GetUsers(); // Return all active users
        }
        public IQueryable<User> GetAgents()
        {
            return _repository.GetAgents(); // Call the repository method to get agents
        }

        public IQueryable<User> GetTeamLeaders()
        {
            return _repository.GetTeamLeaders(); // Call the repository method to get team leaders
        }
    }
}
