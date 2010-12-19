using System;
using System.Collections.Generic;
using System.Linq;
using DevText.Framework.Data;

namespace Post.Model
{
    public class post:EntityBase, IPost
    {
        public virtual string Title{  get;   set;}

        public virtual string Content { get; set; }

        public virtual string Author { get; set; }

        public virtual DateTime Publish { get; set; }
        
    }
}