using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Models
{
    public class MentorProfileModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string UserName { get; set; }

        public List<string> MentorLinks { get; set; }

        public List<string> MentorTags { get; set; }
    }
}
