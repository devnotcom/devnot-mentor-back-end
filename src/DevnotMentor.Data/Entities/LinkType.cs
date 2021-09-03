using System.Collections.Generic;

namespace DevnotMentor.Data.Entities
{
    public partial class LinkType
    {
        public LinkType()
        {
            MenteeLinks = new HashSet<MenteeLink>();
            MentorLinks = new HashSet<MentorLink>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? HasUsername { get; set; }
        public string BaseLink { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<MenteeLink> MenteeLinks { get; set; }
        public virtual ICollection<MentorLink> MentorLinks { get; set; }
    }
}
