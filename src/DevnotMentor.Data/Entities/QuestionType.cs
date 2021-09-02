using System;
using System.Collections.Generic;

namespace DevnotMentor.Data.Entities
{
    public partial class QuestionType
    {
        public QuestionType()
        {
            MentorQuestion = new HashSet<MentorQuestion>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MentorQuestion> MentorQuestion { get; set; }
    }
}
