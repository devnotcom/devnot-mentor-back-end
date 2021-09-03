using System;

namespace DevnotMentor.Data.Entities
{
    public partial class Mentorship
    {
        public int Id { get; set; }
        public int? MentorId { get; set; }
        public int? MenteeId { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public int? State { get; set; }
        public byte? MentorScore { get; set; }
        public string MentorComment { get; set; }
        public byte? MenteeScore { get; set; }
        public string MenteeComment { get; set; }

        public virtual Mentee Mentee { get; set; }
        public virtual Mentor Mentor { get; set; }
    }
}
