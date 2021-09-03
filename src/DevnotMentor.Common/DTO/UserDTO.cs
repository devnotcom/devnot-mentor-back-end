using System.Collections.Generic;
using System.Linq;

namespace DevnotMentor.Common.DTO
{
    public class UserDTO
    {
        public UserDTO()
        {
            Mentee = new List<MenteeDTO>();
            Mentor = new List<MentorDTO>();
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

        public virtual ICollection<MenteeDTO> Mentee { get; set; }
        public virtual ICollection<MentorDTO> Mentor { get; set; }
    }
}
