using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Models
{
    public class SearchMentorModel
    {
        public List<string> Tags { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public int PageIndex { get; set; }

        public int PageNumber { get; set; }
    }
}
