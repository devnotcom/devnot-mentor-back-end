using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using System.Linq;

namespace DevnotMentor.Data
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(MentorDBContext context) : base(context)
        {
        }

        public Tag GetByName(string tagName)
        {
            return DbContext.Tags.Where(i => i.Name == tagName).FirstOrDefault();
        }
    }
}
