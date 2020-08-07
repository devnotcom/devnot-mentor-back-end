using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Models
{
    public class ApplyMentorModel
    {
        public int MenteeUserId { get; set; }
        public int MentorUserId { get; set; }
        public string ApplicationNotes { get; set; }
        public int Status { get; set; }
    }
}
