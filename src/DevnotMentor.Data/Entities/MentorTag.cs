using System;
using System.Collections.Generic;

namespace DevnotMentor.Data.Entities
{
    public partial class MentorTag
    {
        public int Id { get; set; }
        public int? MentorId { get; set; }
        public int? TagId { get; set; }

        public virtual Mentor Mentor { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
