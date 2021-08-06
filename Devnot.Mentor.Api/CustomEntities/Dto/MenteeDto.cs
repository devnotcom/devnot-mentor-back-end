using System.Collections.Generic;

namespace DevnotMentor.Api.CustomEntities.Dto
{
    public class MenteeDto
    {
        public MenteeDto()
        {
            MenteeTags = new List<MenteeTagDto>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }

        public UserDto User { get; set; }
        public List<MenteeTagDto> MenteeTags { get; set; }
    }
}
