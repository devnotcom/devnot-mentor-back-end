using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Models
{
    public class RemindPasswordCompleteModel
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public Guid SecurityKey { get; set; }
    }
}
