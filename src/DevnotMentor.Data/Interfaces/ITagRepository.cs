using DevnotMentor.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Data.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        Tag Get(string tagName);
    }
}
