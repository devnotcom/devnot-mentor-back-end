using System.Collections.Generic;

namespace DevnotMentor.Api.CustomEntities.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }

        public ICollection<MenteeDto> Mentees { get; set; }
        public ICollection<MentorDto> Mentors { get; set; }
    }
}
