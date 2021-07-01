using DevnotMentor.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IMentorApplicationsRepository : IRepository<MentorApplications>
    {
        Task<bool> IsExistsByUserId(int mentorId, int menteeId);
        Task<MentorApplications> Get(int mentorId, int menteeId);
        Task<IEnumerable<MentorApplications>> GetForMentees(Expression<Func<MentorApplications, bool>> predicate);
    }
}
