using System;
using System.Collections.Generic;

namespace DevnotMentor.Api.Entities
{
    public partial class MentorLinks
    {
        public int Id { get; set; }
        public int? MentorId { get; set; }
        public int? LinkTypeId { get; set; }
        public string Link { get; set; }

        public virtual LinkType LinkType { get; set; }
        public virtual Mentor Mentor { get; set; }
    }
}
