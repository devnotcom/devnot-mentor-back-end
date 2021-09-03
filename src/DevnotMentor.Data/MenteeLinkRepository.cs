using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using System.Collections.Generic;

namespace DevnotMentor.Data
{
    public class MenteeLinkRepository : BaseRepository<MenteeLink>, IMenteeLinkRepository
    {
        public MenteeLinkRepository(MentorDBContext context) : base(context)
        {
        }

        public void Create(int mentorId, List<string> list)
        {
            foreach (var item in list)
            {
                Create(new MenteeLink { Link = item, MenteeId = mentorId });
            }
        }
    }
}
