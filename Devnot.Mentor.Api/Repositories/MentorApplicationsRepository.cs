using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class MentorApplicationsRepository : BaseRepository<MentorApplications>, IMentorApplicationsRepository
    {
        public MentorApplicationsRepository(MentorDBContext context) : base(context)
        {

        }

        public async Task<bool> IsExistsByUserId(int mentorId, int menteeId)
        {
            return await DbContext
                .MentorApplications
                .Where(i => i.MentorId == mentorId && i.MenteeId == menteeId)
                .AnyAsync();
        }

        public async Task<MentorApplications> Get(int mentorId, int menteeId)
        {
            return await DbContext
                .MentorApplications
                .Where(i => i.MentorId == mentorId && i.MenteeId == menteeId)
                .FirstOrDefaultAsync();
        }
    }
}
