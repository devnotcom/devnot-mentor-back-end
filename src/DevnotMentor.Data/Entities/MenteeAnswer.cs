namespace DevnotMentor.Data.Entities
{
    public partial class MenteeAnswer
    {
        public int Id { get; set; }
        public int? MenteeId { get; set; }
        public int? MentorQuestionId { get; set; }
        public string Answer { get; set; }

        public virtual Mentee Mentee { get; set; }
        public virtual MentorQuestion MentorQuestion { get; set; }
    }
}
