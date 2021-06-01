using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(MentorDBContext context) : base(context)
        {
        }

        public Tag Get(string tagName)
        {
            return DbContext.Tag.Where(i => i.Name == tagName).FirstOrDefault();
        }
    }
}
