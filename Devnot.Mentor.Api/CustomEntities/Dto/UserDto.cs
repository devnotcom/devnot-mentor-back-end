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
        public string Name { get; set; }
        public string SurName { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool? UserNameConfirmed { get; set; }
        public int? UserState { get; set; }
        public string ProfileUrl { get; set; }

        public bool IsMentee => Mentee.Any();
        public bool IsMentor => Mentor.Any();

        public virtual ICollection<MenteeDto> Mentee { get; set; }
        public virtual ICollection<MentorDto> Mentor { get; set; }
    }
}
