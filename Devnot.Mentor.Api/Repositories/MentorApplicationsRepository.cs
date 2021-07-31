using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class MentorApplicationsRepository : BaseRepository<MentorApplications>, IMentorApplicationsRepository
    {
        public MentorApplicationsRepository(MentorDBContext context) : base(context)
        {

        }

        public async Task<bool> AnyWaitingApplicationBetweenMentorAndMenteeAsync(int mentorId, int menteeId)
        {
            return await DbContext.MentorApplications.AnyAsync(x => 
                x.Status == MentorApplicationStatus.Waiting.ToInt()
                && x.MentorId == mentorId
                && x.MenteeId == menteeId);
        }

        public async Task<IEnumerable<MentorApplications>> GetApplicationsByUserIdAsync(int userId)
        {
            return await DbContext.MentorApplications
                .Include(x => x.Mentee).ThenInclude(x => x.User)
                .Include(x => x.Mentor).ThenInclude(x => x.User)
                .Where(x => x.Mentor.UserId == userId || x.Mentee.UserId == userId)
                .ToListAsync();
        }

        public async Task<MentorApplications> GetWhichIsWaitingByIdAsync(int applicationId)
        {
            return await DbContext.MentorApplications
                .Include(x => x.Mentee).ThenInclude(x => x.User)
                .Include(x => x.Mentor).ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == applicationId && x.Status == MentorApplicationStatus.Waiting.ToInt());
        }
    }
}
