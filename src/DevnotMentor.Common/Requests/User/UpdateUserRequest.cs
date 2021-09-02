using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DevnotMentor.Common.Requests.User
{
    public class UpdateUserRequest
    {
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string SurName { get; set; }

        public string ProfileImageUrl { get; set; }

        public IFormFile ProfileImage { get; set; }
    }
}
