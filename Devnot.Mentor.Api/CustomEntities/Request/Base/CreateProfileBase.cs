using System.Text.Json.Serialization;

namespace DevnotMentor.Api.CustomEntities.Request.Base
{
    public class CreateProfileBase
    {
        public string Title { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Which user wants to create a new profile as mentor or mentee
        /// </summary>
        /// <value>Authenticated User Id - value is passing in controller side</value>
        [JsonIgnore]
        public int UserId { get; set; }
    }
}