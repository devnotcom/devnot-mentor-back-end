using System;
using System.Collections.Generic;

namespace DevnotMentor.Api.Entities
{
    public partial class MentorTags
    {
        public int Id { get; set; }
        public int? MentorId { get; set; }
        public int? TagId { get; set; }

        public virtual Mentor Mentor { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
