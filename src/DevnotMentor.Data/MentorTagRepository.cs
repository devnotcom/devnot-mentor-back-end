using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;

namespace DevnotMentor.Data
{
    public class MentorTagRepository : BaseRepository<MentorTag>, IMentorTagRepository
    {
        public MentorTagRepository(MentorDBContext context) : base(context)
        {
        }
    }
}
