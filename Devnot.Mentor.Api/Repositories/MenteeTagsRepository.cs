using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class MenteeTagsRepository : BaseRepository<MenteeTags>, IMenteeTagsRepository
    {
        public MenteeTagsRepository(MentorDBContext context) : base(context)
        {
        }
    }
}
