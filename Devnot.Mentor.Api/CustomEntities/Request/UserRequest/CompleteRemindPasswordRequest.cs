using System;
using System.ComponentModel.DataAnnotations;

namespace DevnotMentor.Api.CustomEntities.Request.UserRequest
{
    public class CompleteRemindPasswordRequest
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public Guid SecurityKey { get; set; }
    }
}
