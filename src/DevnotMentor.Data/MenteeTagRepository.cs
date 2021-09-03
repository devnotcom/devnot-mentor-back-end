using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;

namespace DevnotMentor.Data
{
    public class MenteeTagRepository : BaseRepository<MenteeTag>, IMenteeTagRepository
    {
        public MenteeTagRepository(MentorDBContext context) : base(context)
        {
        }
    }
}
