using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DevnotMentor.Api.CustomEntities.Request.MentorRequest
{
    public class CreateMentorProfileRequest
    {
        public string Title { get; set; }

        public string Description { get; set; }
        
        [JsonIgnore]
        public int UserId { get; set; }

        public List<string> MentorLinks { get; set; }

        public List<string> MentorTags { get; set; }
    }
}
