using DevnotMentor.Data.Entities;
using System.Collections.Generic;

namespace DevnotMentor.Data.Interfaces
{
    public interface IMentorLinkRepository : IRepository<MentorLink>
    {
        void Create(int mentorId, List<string> list);
    }
}
