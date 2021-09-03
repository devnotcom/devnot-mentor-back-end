using System.Collections.Generic;

namespace DevnotMentor.Data.Entities
{
    public partial class MentorQuestion
    {
        public MentorQuestion()
        {
            MenteeAnswers = new HashSet<MenteeAnswer>();
        }

        public int Id { get; set; }
        public int? MentorId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionNotes { get; set; }
        public int? QuestionTypeId { get; set; }

        public virtual Mentor Mentor { get; set; }
        public virtual QuestionType QuestionType { get; set; }
        public virtual ICollection<MenteeAnswer> MenteeAnswers { get; set; }
    }
}
