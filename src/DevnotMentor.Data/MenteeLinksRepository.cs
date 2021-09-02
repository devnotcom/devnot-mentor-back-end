using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using System.Collections.Generic;

namespace DevnotMentor.Data
{
    public class MenteeLinksRepository : BaseRepository<MenteeLink>, IMenteeLinksRepository
    {
        public MenteeLinksRepository(MentorDBContext context) : base(context)
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
