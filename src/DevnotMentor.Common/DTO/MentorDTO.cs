using System.Collections.Generic;

namespace DevnotMentor.Common.DTO
{
    public class MentorDTO
    {
        public MentorDTO()
        {
            MentorTags = new List<MentorTagDTO>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public UserDTO User { get; set; }
        public List<MentorTagDTO> MentorTags { get; set; }
    }
}
