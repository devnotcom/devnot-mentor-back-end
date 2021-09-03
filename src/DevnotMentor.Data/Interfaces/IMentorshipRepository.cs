using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Data.Entities;

namespace DevnotMentor.Data.Interfaces
{
    public interface IMentorshipRepository : IRepository<Mentorship>
    {
        /// <summary>
        /// Gets total status count which has continuing status by mentee id.
        /// </summary>
        int GetCountForContinuingStatusByMenteeId(int menteeId);
        
        /// <summary>
        /// Gets total status count which has continuing status by mentor id.
        /// </summary>
        int GetCountForContinuingStatusByMentorId(int mentorId);

        /// <summary>
        /// Gets mentor, mentee mentorships by user id.
        /// </summary>
        Task<IEnumerable<Mentorship>> GetMentorshipsByUserIdAsync(int userId);

        /// <summary>
        /// Gets mentor, mentee mentorship which is not finished.
        /// </summary>
        Task<Mentorship> GetWhichIsNotFinishedYetByIdAsync(int mentorshipId);

        /// <summary>
        /// Gets mentor, mentee mentorship which is finished.
        /// </summary>
        Task<Mentorship> GetWhichIsFinishedByIdAsync(int mentorshipId);
    }
}
