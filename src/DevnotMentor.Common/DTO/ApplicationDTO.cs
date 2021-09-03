using System;

namespace DevnotMentor.Common.DTO
{
    public class ApplicationDTO
    {
        public int Id { get; set; }
        public DateTime? AppliedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Note { get; set; }
        public int? Status { get; set; }

        public MentorDTO Mentor { get; set; }
        public MenteeDTO Mentee { get; set; }
    }
}