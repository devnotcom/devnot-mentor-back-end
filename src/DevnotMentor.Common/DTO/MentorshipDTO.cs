using System;

namespace DevnotMentor.Common.DTO
{
    public class MentorshipDTO
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

        public MenteeDTO Mentee { get; set; }
        public MentorDTO Mentor { get; set; }
    }
}