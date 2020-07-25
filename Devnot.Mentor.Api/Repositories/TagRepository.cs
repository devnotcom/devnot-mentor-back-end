using DevnotMentor.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(MentorDBContext context):base(context)
        {
        }
    }
}
