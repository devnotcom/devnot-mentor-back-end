using System;

namespace DevnotMentor.Api.CustomEntities.Dto
{
    public class PairDto
    {
        public int Id { get; set; }
        public int? MentorId { get; set; }
        public int? MenteeId { get; set; }
        public DateTime? MentorStartDate { get; set; }
        public DateTime? MentorEndDate { get; set; }
        public int? State { get; set; }
        public byte? MentorScore { get; set; }
        public string MentorComment { get; set; }
        public byte? MenteeScore { get; set; }
        public string MenteeComment { get; set; }

        public MenteeDto Mentee { get; set; }
        public MentorDto Mentor { get; set; }
    }
}