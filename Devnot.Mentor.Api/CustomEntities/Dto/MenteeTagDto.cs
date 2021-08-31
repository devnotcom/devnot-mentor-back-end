namespace DevnotMentor.Api.CustomEntities.Dto
{
    public class MenteeTagDto
    {
        public int Id { get; set; }
        public int? MenteeId { get; set; }
        public int? TagId { get; set; }

        public TagDto Tag { get; set; }
    }
}
