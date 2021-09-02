using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DevnotMentor.Common.Requests.User
{
    public class RegisterUserRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string SurName { get; set; }

        public string ProfileImageUrl { get; set; }
        public IFormFile ProfileImage { get; set; }
    }
}
