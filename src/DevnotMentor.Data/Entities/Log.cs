using System;

namespace DevnotMentor.Data.Entities
{
    public partial class Log
    {
        public Guid Id { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime InsertedAt { get; set; }
    }
}
