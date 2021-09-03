using System.Collections.Generic;
using DevnotMentor.Common.Requests.Base;

namespace DevnotMentor.Common.Requests.Mentor
{
    public class CreateMentorProfileRequest : CreateProfileBase
    {
        public List<string> MentorLinks { get; set; }

        public List<string> MentorTags { get; set; }
    }
}
