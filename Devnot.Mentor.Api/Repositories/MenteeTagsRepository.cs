using DevnotMentor.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class MenteeTagsRepository:Repository<MenteeTags>
    {
        public MenteeTagsRepository(MentorDBContext context) : base(context)
        {
        }
    }
}
