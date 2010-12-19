using System;
using System.Collections.Generic;
using System.Linq;
using DevText.Framework.Data;
using Post.Model;

namespace Post.Repository
{
    public class postRepository:GenericRepository<post>,IpostRepository
    {
    }
}