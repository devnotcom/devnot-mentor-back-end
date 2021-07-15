using System.ComponentModel.DataAnnotations;

namespace DevnotMentor.Api.CustomEntities.Request.UserRequest
{
    public class UpdateUserRequest
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
