using System.Collections.Generic;
using DevnotMentor.Common.Requests.Base;

namespace DevnotMentor.Common.Requests.Mentee
{
    public class CreateMenteeProfileRequest : CreateProfileBase
    {
        public List<string> MenteeLinks { get; set; }

        public List<string> MenteeTags { get; set; }
    }
}
