using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;

namespace DevnotMentor.Data
{
    public class MentorTagsRepository : BaseRepository<MentorTag>, IMentorTagsRepository
    {
        public MentorTagsRepository(MentorDBContext context) : base(context)
        {
        }
    }
}
