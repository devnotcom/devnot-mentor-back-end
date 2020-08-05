using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Enums
{
    public enum MentorMenteePairStatus
    {
        Waiting,
        Approved,
        Rejected
    }

    public static class MentorMenteePairStatusExtensions
    {
        public static int? ToInt(this MentorMenteePairStatus status)
        {
            return (int?)status;
        }
    }

}
