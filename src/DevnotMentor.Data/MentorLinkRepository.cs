using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using System.Collections.Generic;

namespace DevnotMentor.Data
{
    public class MentorLinkRepository : BaseRepository<MentorLink>, IMentorLinkRepository
    {
        public MentorLinkRepository(MentorDBContext context) : base(context)
        {
        }

        public void Create(int mentorId, List<string> list)
        {
            foreach (var item in list)
            {
                Create(new MentorLink { Link = item, MentorId = mentorId });
            }
        }
    }
}
