using System;

namespace DevnotMentor.Api.CustomEntities.Dto
{
    public class MentorApplicationsDto
    {
        public int Id { get; set; }
        public DateTime? ApplyDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string ApllicationNotes { get; set; }
        public int? Status { get; set; }

        public MentorDto Mentor { get; set; }
        public MenteeDto Mentee { get; set; }
    }
}