using System.ComponentModel.DataAnnotations;

namespace DevnotMentor.Common.Requests.Mentorship
{
    public class MentorshipFeedbackRequest
    {
        [Required]
        [Range(0, 5)]
        public byte Score { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Comment { get; set; }
    }
}