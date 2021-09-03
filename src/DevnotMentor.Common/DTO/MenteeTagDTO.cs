namespace DevnotMentor.Common.DTO
{
    public class MenteeTagDTO
    {
        public int Id { get; set; }
        public int? MenteeId { get; set; }
        public int? TagId { get; set; }

        public TagDTO Tag { get; set; }
    }
}
