using System;
using System.Collections.Generic;

namespace DevnotMentor.Api.Entities
{
    public partial class Mentee
    {
        public Mentee()
        {
            MenteeAnswers = new HashSet<MenteeAnswers>();
            MenteeLinks = new HashSet<MenteeLinks>();
            MenteeTags = new HashSet<MenteeTags>();
            MentorApplications = new HashSet<MentorApplications>();
            MentorMenteePairs = new HashSet<MentorMenteePairs>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<MenteeAnswers> MenteeAnswers { get; set; }
        public virtual ICollection<MenteeLinks> MenteeLinks { get; set; }
        public virtual ICollection<MenteeTags> MenteeTags { get; set; }
        public virtual ICollection<MentorApplications> MentorApplications { get; set; }
        public virtual ICollection<MentorMenteePairs> MentorMenteePairs { get; set; }
    }
}
