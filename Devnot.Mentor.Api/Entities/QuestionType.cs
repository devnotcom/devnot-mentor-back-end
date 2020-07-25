using System;
using System.Collections.Generic;

namespace DevnotMentor.Api.Entities
{
    public partial class QuestionType
    {
        public QuestionType()
        {
            MentorQuestions = new HashSet<MentorQuestions>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MentorQuestions> MentorQuestions { get; set; }
    }
}
