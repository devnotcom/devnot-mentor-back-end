using System;
using System.Collections.Generic;

namespace DevnotMentor.Api.Entities
{
    public partial class MentorQuestions
    {
        public MentorQuestions()
        {
            MenteeAnswers = new HashSet<MenteeAnswers>();
        }

        public int Id { get; set; }
        public int? MentorId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionNotes { get; set; }
        public int? QuestionTypeId { get; set; }

        public virtual Mentor Mentor { get; set; }
        public virtual QuestionType QuestionType { get; set; }
        public virtual ICollection<MenteeAnswers> MenteeAnswers { get; set; }
    }
}
