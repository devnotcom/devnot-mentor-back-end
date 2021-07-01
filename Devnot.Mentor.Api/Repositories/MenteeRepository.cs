using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class MenteeRepository : BaseRepository<Mentee>, IMenteeRepository
    {

        public MenteeRepository(MentorDBContext context) : base(context)
        {

        }

        public async Task<Mentee> GetByUserId(int userId)
        {
            return await DbContext.Mentee.Where(i => i.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<int> GetIdByUserId(int userId)
        {
            return await DbContext.Mentee.Where(i => i.UserId == userId).Select(i => i.Id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsByUserId(int userId)
        {
            return await DbContext.Mentee.AnyAsync(i => i.UserId == userId);
        }

        public async Task<IEnumerable<Mentor>> GetMentors(Expression<Func<MentorMenteePairs, bool>> predicate)
        {
            return await DbContext.MentorMenteePairs.Where(predicate).Include(x => x.Mentor).ThenInclude(x => x.User).Select(x => x.Mentor).ToListAsync();
        }
    }
}
