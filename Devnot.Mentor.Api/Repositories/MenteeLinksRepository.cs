using DevnotMentor.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class MenteeLinksRepository:Repository<MenteeLinks>
    {
        public MenteeLinksRepository(MentorDBContext context) : base(context)
        {
        }

        public void Create(int mentorId, List<string> list)
        {
            foreach (var item in list)
            {
                Create(new MenteeLinks { Link = item, MenteeId = mentorId });
            }
        }
    }
}
