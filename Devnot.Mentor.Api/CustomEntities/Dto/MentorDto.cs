namespace DevnotMentor.Api.CustomEntities.Dto
{
    public class MentorDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserDto User { get; set; }
    }
}
