using System;
using System.Collections.Generic;

namespace DevnotMentor.Data.Entities
{
    public partial class Mentee
    {
        public Mentee()
        {
            MenteeAnswers = new HashSet<MenteeAnswer>();
            MenteeLinks = new HashSet<MenteeLink>();
            MenteeTags = new HashSet<MenteeTag>();
            Applications = new HashSet<Application>();
            Mentorships = new HashSet<Mentorship>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<MenteeAnswer> MenteeAnswers { get; set; }
        public virtual ICollection<MenteeLink> MenteeLinks { get; set; }
        public virtual ICollection<MenteeTag> MenteeTags { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<Mentorship> Mentorships { get; set; }
    }
}
