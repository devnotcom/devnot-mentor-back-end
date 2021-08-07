using System.Collections.Generic;
using System.Linq;

namespace DevnotMentor.Api.CustomEntities.Dto
{
    public class UserDto
    {
        public UserDto()
        {
            Mentee = new List<MenteeDto>();
            Mentor = new List<MentorDto>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }

        public bool IsMentee => Mentee.Any();
        public bool IsMentor => Mentor.Any();

        public virtual ICollection<MenteeDto> Mentee { get; set; }
        public virtual ICollection<MentorDto> Mentor { get; set; }
    }
}
