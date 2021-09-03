using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using System.Collections.Generic;

namespace DevnotMentor.Data
{
    public class MentorLinkRepository : BaseRepository<MentorLink>, IMentorLinkRepository
    {
        public MentorLinkRepository(MentorDBContext context) : base(context) { }

        public void Create(int mentorId, List<string> links)
        {
            foreach (var link in links)
            {
                Create(new MentorLink { Link = link, MentorId = mentorId });
            }
        }
    }
}
