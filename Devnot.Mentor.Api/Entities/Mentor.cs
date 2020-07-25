using System;
using System.Collections.Generic;

namespace DevnotMentor.Api.Entities
{
    public partial class Mentor
    {
        public Mentor()
        {
            MentorApplications = new HashSet<MentorApplications>();
            MentorLinks = new HashSet<MentorLinks>();
            MentorMenteePairs = new HashSet<MentorMenteePairs>();
            MentorQuestions = new HashSet<MentorQuestions>();
            MentorTags = new HashSet<MentorTags>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<MentorApplications> MentorApplications { get; set; }
        public virtual ICollection<MentorLinks> MentorLinks { get; set; }
        public virtual ICollection<MentorMenteePairs> MentorMenteePairs { get; set; }
        public virtual ICollection<MentorQuestions> MentorQuestions { get; set; }
        public virtual ICollection<MentorTags> MentorTags { get; set; }
    }
}
