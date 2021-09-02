namespace DevnotMentor.Data.Entities
{
    public partial class MenteeTag
    {
        public int Id { get; set; }
        public int? MenteeId { get; set; }
        public int? TagId { get; set; }

        public virtual Mentee Mentee { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
