using DevnotMentor.Data.Entities;
using System.Collections.Generic;

namespace DevnotMentor.Data.Interfaces
{
    public interface IMenteeLinkRepository : IRepository<MenteeLink>
    {
        void Create(int mentorId, List<string> list);
    }
}
