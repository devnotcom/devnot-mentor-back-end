using System.Collections.Generic;
using DevnotMentor.Api.CustomEntities.Request.Base;

namespace DevnotMentor.Api.CustomEntities.Request.MenteeRequest
{
    public class CreateMenteeProfileRequest : CreateProfileBase
    {
        public List<string> MenteeLinks { get; set; }

        public List<string> MenteeTags { get; set; }
    }
}
