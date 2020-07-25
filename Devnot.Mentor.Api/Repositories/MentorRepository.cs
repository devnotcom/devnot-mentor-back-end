using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevnotMentor.Api.Entities;

namespace DevnotMentor.Api.Repositories
{
    public class MentorRepository : Repository<Mentor>
    {
        public MentorRepository(MentorDBContext context) :base(context)
        {
        }
    }
}
