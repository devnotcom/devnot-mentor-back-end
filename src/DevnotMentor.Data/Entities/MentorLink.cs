namespace DevnotMentor.Data.Entities
{
    public partial class MentorLink
    {
        public int Id { get; set; }
        public int? MentorId { get; set; }
        public int? LinkTypeId { get; set; }
        public string Link { get; set; }

        public virtual LinkType LinkType { get; set; }
        public virtual Mentor Mentor { get; set; }
    }
}
