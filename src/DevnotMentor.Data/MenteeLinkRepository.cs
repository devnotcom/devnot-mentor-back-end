using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using System.Collections.Generic;

namespace DevnotMentor.Data
{
    public class MenteeLinkRepository : BaseRepository<MenteeLink>, IMenteeLinkRepository
    {
        public MenteeLinkRepository(MentorDBContext context) : base(context) { }

        public void Create(int menteeId, List<string> links)
        {
            foreach (var link in links)
            {
                Create(new MenteeLink { Link = link, MenteeId = menteeId });
            }
        }
    }
}
