
namespace DevnotMentor.Api.CustomEntities.Request.MentorRequest
{
    public class ApplyToMentorRequest
    {
        public int MenteeUserId { get; set; }
        public int MentorUserId { get; set; }
        public string ApplicationNotes { get; set; }
    }
}