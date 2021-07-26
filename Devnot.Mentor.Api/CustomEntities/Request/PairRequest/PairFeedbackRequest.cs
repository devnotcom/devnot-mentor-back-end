using System.ComponentModel.DataAnnotations;

namespace DevnotMentor.Api.CustomEntities.Request.PairRequest
{
    public class PairFeedbackRequest
    {
        [Required]
        [Range(0, 5)]
        public byte Score { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Comment { get; set; }
    }
}