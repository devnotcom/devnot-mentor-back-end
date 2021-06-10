using System.Collections.Generic;

namespace DevnotMentor.Api.CustomEntities.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool? UserNameConfirmed { get; set; }
        public int? UserState { get; set; }
        public string ProfileUrl { get; set; }

        public ICollection<MenteeDto> Mentees { get; set; }
        public ICollection<MentorDto> Mentors { get; set; }
    }
}
