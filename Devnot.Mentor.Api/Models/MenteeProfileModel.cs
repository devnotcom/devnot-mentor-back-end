using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Models
{
    public class MenteeProfileModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string UserName { get; set; }

        public List<string> MenteeLinks { get; set; }

        public List<string> MenteeTags { get; set; }
    }
}
