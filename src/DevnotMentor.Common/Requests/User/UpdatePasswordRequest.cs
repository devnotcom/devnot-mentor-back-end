using System.ComponentModel.DataAnnotations;

namespace DevnotMentor.Common.Requests.User
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
