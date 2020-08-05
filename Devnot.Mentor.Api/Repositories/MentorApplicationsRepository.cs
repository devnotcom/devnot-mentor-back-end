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
        private MentorDBContext _context;
        public MentorApplicationsRepository(MentorDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> AnyPairByUserIdAsync(int mentorUserId, int menteeUserId)
        {
            return await _context
                .MentorApplications
                .Where(i => i.Mentor.UserId == mentorUserId && i.Mentee.UserId == menteeUserId)
                .AnyAsync();
        }

        public async Task<MentorApplications> GetAsync(int mentorUserId, int menteeUserId)
        {
            return await _context
                .MentorApplications
                .Where(i => i.Mentor.User.Id == mentorUserId && i.Mentee.User.Id == menteeUserId)
                .FirstOrDefaultAsync();
        }
    }
}
