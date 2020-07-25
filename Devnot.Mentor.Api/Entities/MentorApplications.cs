using System;
using System.Collections.Generic;

namespace DevnotMentor.Api.Entities
{
    public partial class MentorApplications
    {
        public int Id { get; set; }
        public int? MenteeId { get; set; }
        public int? MentorId { get; set; }
        public DateTime? ApplyDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string ApllicationNotes { get; set; }
        public int? Status { get; set; }

        public virtual Mentee Mentee { get; set; }
        public virtual Mentor Mentor { get; set; }
    }
}
