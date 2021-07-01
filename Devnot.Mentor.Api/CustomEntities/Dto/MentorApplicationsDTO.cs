using System;

namespace DevnotMentor.Api.CustomEntities.Dto
{
    public class MentorApplicationsDTO
    {
        public DateTime? ApplyDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string ApllicationNotes { get; set; }
        public int? Status { get; set; }

        public MentorDto Mentor { get; set; }
        public MenteeDto Mentee { get; set; }
    }
}