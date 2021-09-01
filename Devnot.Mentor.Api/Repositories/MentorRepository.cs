using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevnotMentor.Api.CustomEntities.Request.CommonRequest;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevnotMentor.Api.Repositories
{
    public class MentorRepository : BaseRepository<Mentor>, IMentorRepository
    {
        public MentorRepository(MentorDBContext context) : base(context)
        {
        }

        public Task<List<Mentor>> SearchAsync(SearchRequest request)
        {
            if (request is null || request.IsNotValid())
            {
                return DbContext
                    .Mentor
                    .Include(mentor => mentor.User)
                    .Include(mentor => mentor.MentorTags)
                        .ThenInclude(mentorTag => mentorTag.Tag)
                    .ToListAsync();
            }

            var queryableMentor = DbContext.Mentor.AsQueryable();

            if (!string.IsNullOrEmpty(request.FullName))
            {
                queryableMentor = queryableMentor.Where(mentor => (mentor.User.Name + " " + mentor.User.SurName).Contains(request.FullName));
            }

            if (!string.IsNullOrEmpty(request.Title))
            {
                queryableMentor = queryableMentor.Where(mentor => mentor.Title.Contains(request.Title));
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                queryableMentor = queryableMentor.Where(mentor => mentor.Description.Contains(request.Description));
            }

            if (request.Tags.Any())
            {
                queryableMentor = queryableMentor.Where(mentor => mentor.MentorTags.Any(tags => request.Tags.Contains(tags.Tag.Name)));
            }

            return queryableMentor
                .Include(mentor => mentor.User)
                .Include(mentor => mentor.MentorTags)
                    .ThenInclude(mentorTag => mentorTag.Tag)
                .ToListAsync();
        }

        public async Task<int> GetIdByUserIdAsync(int userId)
        {
            return await DbContext.Mentor.Where(i => i.UserId == userId).Select(i => i.Id).FirstOrDefaultAsync();
        }

        public async Task<Mentor> GetByUserNameAsync(string userName)
        {
            return await DbContext.Mentor.Include(x => x.User).Where(i => i.User.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<Mentor> GetByUserIdAsync(int userId)
        {
            return await DbContext.Mentor.Where(i => i.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsByUserIdAsync(int userId)
        {
            return await DbContext.Mentor.AnyAsync(i => i.UserId == userId);
        }

        public async Task<IEnumerable<Mentee>> GetPairedMenteesByMentorIdAsync(int mentorId)
        {
            return await DbContext.MentorMenteePairs.Where(x => x.MentorId == mentorId)
                .Include(x => x.Mentee)
                .ThenInclude(x => x.User)
                .Select(x => x.Mentee)
                .ToListAsync();
        }

        public async Task<Mentor> GetByIdAsync(int mentorId)
        {
            return await DbContext.Mentor.Include(x=>x.User).Where(x => x.Id == mentorId).FirstOrDefaultAsync();
        }
    }
}
