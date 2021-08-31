namespace DevnotMentor.Api.CustomEntities.Dto
{
    public class MentorTagDto
    {
        public int Id { get; set; }
        public int? MentorId { get; set; }
        public int? TagId { get; set; }

        public TagDto Tag { get; set; }
    }
}
