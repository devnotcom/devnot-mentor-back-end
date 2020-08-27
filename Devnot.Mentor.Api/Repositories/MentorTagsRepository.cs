using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class MentorTagsRepository : BaseRepository<MentorTags>, IMentorTagsRepository
    {
        public MentorTagsRepository(MentorDBContext context) : base(context)
        {
        }
    }
}
