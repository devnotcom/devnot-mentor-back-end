using System;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevnotMentor.Api.CustomEntities.Request.CommonRequest;

namespace DevnotMentor.Api.Repositories
{
    public class MenteeRepository : BaseRepository<Mentee>, IMenteeRepository
    {

        public MenteeRepository(MentorDBContext context) : base(context)
        {

        }

        public Task<List<Mentee>> SearchAsync(SearchRequest request)
        {
            if (request is null || request.IsNotValid())
            {
                return DbContext
                    .Mentee
                    .Include(mentee => mentee.User)
                    .Include(mentee => mentee.MenteeTags)
                        .ThenInclude(menteeTag => menteeTag.Tag)
                    .ToListAsync();
            }

            var queryableMentee = DbContext.Mentee.AsQueryable();

            if (!string.IsNullOrEmpty(request.FullName))
            {
                queryableMentee = queryableMentee.Where(mentee => (mentee.User.Name + " " + mentee.User.SurName).Contains(request.FullName));
            }

            if (!string.IsNullOrEmpty(request.Title))
            {
                queryableMentee = queryableMentee.Where(mentee => mentee.Title.Contains(request.Title));
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                queryableMentee = queryableMentee.Where(mentee => mentee.Description.Contains(request.Description));
            }

            if (request.Tags.Any())
            {
                queryableMentee = queryableMentee.Where(mentee => mentee.MenteeTags.Any(tags => request.Tags.Contains(tags.Tag.Name)));
            }

            return queryableMentee
                .Include(mentee => mentee.User)
                .Include(mentee => mentee.MenteeTags)
                    .ThenInclude(menteeTag => menteeTag.Tag)
                .ToListAsync();
        }

        public async Task<Mentee> GetByUserIdAsync(int userId)
        {
            return await DbContext.Mentee.Where(i => i.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<Mentee> GetByUserNameAsync(string userName)
        {
            return await DbContext.Mentee.Include(x => x.User).Where(i => i.User.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<int> GetIdByUserIdAsync(int userId)
        {
            return await DbContext.Mentee.Where(i => i.UserId == userId).Select(i => i.Id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsByUserIdAsync(int userId)
        {
            return await DbContext.Mentee.AnyAsync(i => i.UserId == userId);
        }

        public async Task<IEnumerable<Mentor>> GetPairedMentorsByMenteeIdAsync(int menteeId)
        {
            return await DbContext.MentorMenteePairs.Where(x => x.MenteeId == menteeId)
                .Include(x => x.Mentor)
                .ThenInclude(x => x.User)
                .Select(x => x.Mentor)
                .ToListAsync();
        }

        public async Task<Mentee> GetByIdAsync(int menteeId)
        {
            return await DbContext.Mentee.Include(x=>x.User).Where(x => x.Id == menteeId).FirstOrDefaultAsync();
        }
    }
}
