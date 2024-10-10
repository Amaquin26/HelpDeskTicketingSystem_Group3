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
                user.Email = model.Email;
                _repository.AddUser(user);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserExists);
            }
        }
        public List<User> GetUsers()
        {
            // Filter only active users (IsInactive = false)
            return _repository.GetUsers().Where(u => !u.IsActive).ToList();
        }
        public void DeleteUser(User user)
        {
            var existingUser = _repository.GetUsers().FirstOrDefault(x => x.UserId == user.UserId);
            if (existingUser != null)
            {
                existingUser.IsActive = false; // Soft delete by marking as inactive
                _repository.UpdateUser(existingUser); // Use repository to update the user
            }
        }
        public void UpdateUser(User user)
        {
            var existingUser = _repository.GetUsers().FirstOrDefault(x => x.UserId == user.UserId);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Password = PasswordManager.EncryptPassword(user.Password); // Encrypt the password
                existingUser.Email = user.Email;
                existingUser.UpdatedTime = DateTime.Now; // Update timestamp
                existingUser.UpdatedBy = Environment.UserName; // Update who changed it

                _repository.UpdateUser(existingUser); // Use repository to update the user
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
    }
}
