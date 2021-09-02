using System.Collections.Generic;
using System.Threading.Tasks;
using DevnotMentor.Data.Entities;

namespace DevnotMentor.Data.Interfaces
{
    public interface IMentorshipsRepository : IRepository<Mentorship>
    {
        /// <summary>
        /// Get total status count which has continuing status by mentee id.
        /// </summary>
        /// <param name="menteeId"></param>
        /// <returns></returns>

        int GetCountForContinuingStatusByMenteeId(int menteeId);
        /// <summary>
        /// Get total status count which has continuing status by mentor id.
        /// </summary>
        /// <param name="mentorId"></param>
        /// <returns></returns>
        int GetCountForContinuingStatusByMentorId(int mentorId);

        /// <summary>
        /// Get mentor mentee pair list for specified user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Mentorship>> GetPairsByUserIdAsync(int userId);

        /// <summary>
        /// Get mentor mentee pair which is not finished.
        /// </summary>
        /// <param name="pairId">Pair id.</param>
        /// <returns></returns>
        Task<Mentorship> GetWhichIsNotFinishedYetByIdAsync(int pairId);

        /// <summary>
        /// Get mentor mentee pair which is finished.
        /// </summary>
        /// <param name="pairsId"></param>
        /// <returns>Pair id.</returns>
        Task<Mentorship> GetWhichIsFinishedByIdAsync(int pairsId);
    }
}
