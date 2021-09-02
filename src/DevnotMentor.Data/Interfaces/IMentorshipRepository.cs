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
        /// <param name="menteeId"></param>
        /// <returns></returns>
        int GetCountForContinuingStatusByMenteeId(int menteeId);
        
        /// <summary>
        /// Gets total status count which has continuing status by mentor id.
        /// </summary>
        /// <param name="mentorId"></param>
        /// <returns></returns>
        int GetCountForContinuingStatusByMentorId(int mentorId);

        /// <summary>
        /// Gets mentor mentee pair list for specified user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Mentorship>> GetMentorshipsByUserIdAsync(int userId);

        /// <summary>
        /// Gets mentor, mentee pair which is not finished.
        /// </summary>
        /// <param name="pairId">Pair id.</param>
        /// <returns></returns>
        Task<Mentorship> GetWhichIsNotFinishedYetByIdAsync(int mentorshipId);

        /// <summary>
        /// Gets mentor, mentee pair which is finished.
        /// </summary>
        /// <param name="pairsId"></param>
        /// <returns>Pair id.</returns>
        Task<Mentorship> GetWhichIsFinishedByIdAsync(int pairsId);
    }
}
