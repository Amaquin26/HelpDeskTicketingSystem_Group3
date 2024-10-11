using ASI.Basecode.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASI.Basecode.WebApp.Models
{
    public class UserModels
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } // Added Email property

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } // Password field

        [Required(ErrorMessage = "Role is required.")]
        public int RoleId { get; set; }

        public bool IsActive { get; set; } = true; // Indicates if the user is active

        public List<User> Agents { get; set; }
    }
}
