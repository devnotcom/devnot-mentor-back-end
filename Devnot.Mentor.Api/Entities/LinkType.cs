using System;
using System.Collections.Generic;

namespace DevnotMentor.Api.Entities
{
    public partial class LinkType
    {
        public LinkType()
        {
            MenteeLinks = new HashSet<MenteeLinks>();
            MentorLinks = new HashSet<MentorLinks>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? HasUsername { get; set; }
        public string BaseLink { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<MenteeLinks> MenteeLinks { get; set; }
        public virtual ICollection<MentorLinks> MentorLinks { get; set; }
    }
}
