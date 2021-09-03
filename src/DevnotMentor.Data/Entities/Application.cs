using System;

namespace DevnotMentor.Data.Entities
{
    public partial class Application
    {
        public int Id { get; set; }
        public int? MenteeId { get; set; }
        public int? MentorId { get; set; }
        public DateTime? AppliedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Note { get; set; }
        public int? Status { get; set; }

        public virtual Mentee Mentee { get; set; }
        public virtual Mentor Mentor { get; set; }
    }
}
