using System;
using System.Collections.Generic;

namespace DevnotMentor.Api.Entities
{
    public partial class Tag
    {
        public Tag()
        {
            MenteeTags = new HashSet<MenteeTags>();
            MentorTags = new HashSet<MentorTags>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MenteeTags> MenteeTags { get; set; }
        public virtual ICollection<MentorTags> MentorTags { get; set; }
    }
}
