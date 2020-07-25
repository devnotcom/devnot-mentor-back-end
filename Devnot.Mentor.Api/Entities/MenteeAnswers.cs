using System;
using System.Collections.Generic;

namespace DevnotMentor.Api.Entities
{
    public partial class MenteeAnswers
    {
        public int Id { get; set; }
        public int? MenteeId { get; set; }
        public int? MentorQuestionId { get; set; }
        public string Answer { get; set; }

        public virtual Mentee Mentee { get; set; }
        public virtual MentorQuestions MentorQuestion { get; set; }
    }
}
