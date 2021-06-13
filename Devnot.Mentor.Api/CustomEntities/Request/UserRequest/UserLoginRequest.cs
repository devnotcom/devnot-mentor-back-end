using System.ComponentModel.DataAnnotations;

namespace DevnotMentor.Api.CustomEntities.Request.UserRequest
{
    public class UserLoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
