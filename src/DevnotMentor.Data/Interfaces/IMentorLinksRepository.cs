using DevnotMentor.Data.Entities;
using System.Collections.Generic;

namespace DevnotMentor.Data.Interfaces
{
    public interface IMentorLinksRepository : IRepository<MentorLink>
    {
        void Create(int mentorId, List<string> list);
    }
}
