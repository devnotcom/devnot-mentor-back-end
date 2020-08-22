using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Models
{
    public class PasswordUpdateModel
    {
        public int UserId { get; set; }
        [Required]
        public string LastPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
