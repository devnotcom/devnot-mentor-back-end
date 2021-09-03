namespace DevnotMentor.Common.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool? UserNameConfirmed { get; set; }
        public int? UserState { get; set; }
        public string ProfileUrl { get; set; }

        /* todo: hold this values on database
        public bool IsMentee => Mentee.Any();
        public bool IsMentor => Mentor.Any(); */
    }
}
