using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;

namespace DevnotMentor.Data
{
    public class MenteeTagsRepository : BaseRepository<MenteeTag>, IMenteeTagsRepository
    {
        public MenteeTagsRepository(MentorDBContext context) : base(context)
        {
        }
    }
}
