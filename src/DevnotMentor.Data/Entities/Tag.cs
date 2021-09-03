using System;
using System.Collections.Generic;

namespace DevnotMentor.Data.Entities
{
    public partial class Tag
    {
        public Tag()
        {
            MenteeTags = new HashSet<MenteeTag>();
            MentorTags = new HashSet<MentorTag>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MenteeTag> MenteeTags { get; set; }
        public virtual ICollection<MentorTag> MentorTags { get; set; }
    }
}
