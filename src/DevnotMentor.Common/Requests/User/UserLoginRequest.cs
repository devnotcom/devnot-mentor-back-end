using System.ComponentModel.DataAnnotations;

namespace DevnotMentor.Common.Requests.User
{
    public class UserLoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
