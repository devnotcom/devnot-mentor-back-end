using System.Collections.Generic;

namespace DevnotMentor.Api.CustomEntities.Dto
{
    public class MentorDto
    {
        public MentorDto()
        {
            MentorTags = new List<MentorTagDto>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public UserDto User { get; set; }
        public List<MentorTagDto> MentorTags { get; set; }
    }
}
