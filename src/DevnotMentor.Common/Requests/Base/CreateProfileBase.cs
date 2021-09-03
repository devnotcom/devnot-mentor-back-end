using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DevnotMentor.Common.Requests.Base
{
    public class CreateProfileBase
    {
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Which user wants to create a new profile as mentor or mentee
        /// </summary>
        /// <value>Authenticated User Id - value is passing in controller side</value>
        [JsonIgnore]
        public int UserId { get; set; }
    }
}