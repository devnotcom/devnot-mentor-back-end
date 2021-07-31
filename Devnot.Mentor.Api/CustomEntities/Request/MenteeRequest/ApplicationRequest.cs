using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DevnotMentor.Api.CustomEntities.Request.MenteeRequest
{
    public class ApplicationRequest
    {
        /// <summary>
        /// Which mentee wants to create application
        /// </summary>
        /// <value>Authorized Mentee User Id - value is passing in controller side</value>
        [JsonIgnore]
        public int MenteeUserId { get; set; }

        [Required]
        public int MentorUserId { get; set; }

        [Required]
        [MaxLength(500)]
        public string ApplicationNotes { get; set; }
    }
}
