using DevnotMentor.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IMentorLinksRepository : IRepository<MentorLinks>
    {
        void Create(int mentorId, List<string> list);
    }
}
