using System.ComponentModel.DataAnnotations;

namespace DevnotMentor.Api.CustomEntities.Request.UserRequest
{
    public class UpdatePasswordRequest
    {
        public int UserId { get; set; }

        [Required]
        public string LastPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
