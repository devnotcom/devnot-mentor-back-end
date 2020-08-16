using System;
using System.Collections.Generic;

namespace DevnotMentor.Api.Entities
{
    public partial class User
    {
        public User()
        {
            Mentee = new HashSet<Mentee>();
            Mentor = new HashSet<Mentor>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool? UserNameConfirmed { get; set; }
        public int? UserState { get; set; }
        public string Token { get; set; }
        public DateTime? TokenExpireDate { get; set; }
        public string ProfileUrl { get; set; }
        public Guid? SecurityKey { get; set; }

        public virtual ICollection<Mentee> Mentee { get; set; }
        public virtual ICollection<Mentor> Mentor { get; set; }
    }
}
