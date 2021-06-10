using System.Collections.Generic;

namespace DevnotMentor.Api.CustomEntities.Request.MenteeRequest
{
    public class CreateMenteeProfileRequest
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public List<string> MenteeLinks { get; set; }

        public List<string> MenteeTags { get; set; }
    }
}
