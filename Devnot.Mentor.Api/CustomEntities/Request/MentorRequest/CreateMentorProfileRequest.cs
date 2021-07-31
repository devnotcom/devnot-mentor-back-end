using System.Collections.Generic;
using DevnotMentor.Api.CustomEntities.Request.Base;

namespace DevnotMentor.Api.CustomEntities.Request.MentorRequest
{
    public class CreateMentorProfileRequest : CreateProfileBase
    {
        public List<string> MentorLinks { get; set; }

        public List<string> MentorTags { get; set; }
    }
}
