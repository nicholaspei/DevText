using System;
using System.Collections.Generic;
using System.Linq;
using DevText.Framework.Data;

namespace Post.Model
{
    public interface IPost:IEntity
    {
        string Title { get; set; }
        string Content { get; set; }
        string Author { get; set; }
        DateTime Publish { get; set; }
    }
}
