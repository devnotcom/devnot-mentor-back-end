using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DevnotMentor.Api.CustomEntities.Request.MenteeRequest
{
    public class CreateMenteeProfileRequest
    {
        public string Title { get; set; }

        public string Description { get; set; }
        
        [JsonIgnore]
        public int UserId { get; set; }

        public List<string> MenteeLinks { get; set; }

        public List<string> MenteeTags { get; set; }
    }
}
