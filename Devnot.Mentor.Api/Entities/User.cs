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
        public string GitHubId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int? UserState { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Token { get; set; }
        public DateTime? TokenExpireDate { get; set; }

        public virtual ICollection<Mentee> Mentee { get; set; }
        public virtual ICollection<Mentor> Mentor { get; set; }
    }
}
