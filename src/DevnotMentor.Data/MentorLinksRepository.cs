using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using System.Collections.Generic;

namespace DevnotMentor.Data
{
    public class MentorLinksRepository : BaseRepository<MentorLink>, IMentorLinksRepository
    {
        public MentorLinksRepository(MentorDBContext context) : base(context)
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
