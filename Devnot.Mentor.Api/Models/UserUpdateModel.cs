using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Models
{
    public class UserUpdateModel
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
