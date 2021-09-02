using System;
using System.Collections.Generic;

namespace DevnotMentor.Data.Entities
{
    public partial class Mentor
    {
        public Mentor()
        {
            Applications = new HashSet<Application>();
            MentorLinks = new HashSet<MentorLink>();
            Mentorships = new HashSet<Mentorship>();
            MentorQuestion = new HashSet<MentorQuestion>();
            MentorTags = new HashSet<MentorTag>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<MentorLink> MentorLinks { get; set; }
        public virtual ICollection<Mentorship> Mentorships { get; set; }
        public virtual ICollection<MentorQuestion> MentorQuestion { get; set; }
        public virtual ICollection<MentorTag> MentorTags { get; set; }
    }
}
