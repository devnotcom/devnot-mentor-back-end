using System;
using System.Collections.Generic;

namespace DevnotMentor.Api.Entities
{
    public partial class Log
    {
        public Guid Id { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
