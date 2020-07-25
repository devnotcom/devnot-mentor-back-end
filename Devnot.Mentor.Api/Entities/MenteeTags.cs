using System;
using System.Collections.Generic;

namespace DevnotMentor.Api.Entities
{
    public partial class MenteeTags
    {
        public int Id { get; set; }
        public int? MenteeId { get; set; }
        public int? TagId { get; set; }

        public virtual Mentee Mentee { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
