using System.Collections.Generic;

namespace DevnotMentor.Common.DTO
{
    public class MenteeDTO
    {
        public MenteeDTO()
        {
            MenteeTags = new List<MenteeTagDTO>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }

        public UserDTO User { get; set; }
        public List<MenteeTagDTO> MenteeTags { get; set; }
    }
}
