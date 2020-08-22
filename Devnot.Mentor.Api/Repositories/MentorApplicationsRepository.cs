using DevnotMentor.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class MentorApplicationsRepository : Repository<MentorApplications>
    {
        public MentorApplicationsRepository(MentorDBContext context) : base(context)
        {

        }

        public async Task<bool> IsExistsByUserId(int mentorUserId, int menteeUserId)
        {
            return await context
                .MentorApplications
                .Where(i => i.Mentor.UserId == mentorUserId && i.Mentee.UserId == menteeUserId)
                .AnyAsync();
        }

        public async Task<MentorApplications> Get(int mentorUserId, int menteeUserId)
        {
            return await context
                .MentorApplications
                .Where(i => i.Mentor.User.Id == mentorUserId && i.Mentee.User.Id == menteeUserId)
                .FirstOrDefaultAsync();
        }
    }
}
