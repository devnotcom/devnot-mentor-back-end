﻿using DevnotMentor.Data.Entities;

namespace DevnotMentor.Data.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        Tag Get(string tagName);
    }
}
