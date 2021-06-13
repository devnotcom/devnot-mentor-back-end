namespace DevnotMentor.Api.CustomEntities.Dto
{
    public class MenteeDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }

        public UserDto User { get; set; }
    }
}
