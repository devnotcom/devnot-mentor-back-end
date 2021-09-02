using System;
using System.ComponentModel.DataAnnotations;

namespace DevnotMentor.Common.Requests.User
{
    public class CompleteRemindPasswordRequest
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public Guid SecurityKey { get; set; }
    }
}
