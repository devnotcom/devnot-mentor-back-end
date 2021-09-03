namespace DevnotMentor.Common.DTO
{
    public class MentorTagDTO
    {
        public int Id { get; set; }
        public int? MentorId { get; set; }
        public int? TagId { get; set; }

        public TagDTO Tag { get; set; }
    }
}
