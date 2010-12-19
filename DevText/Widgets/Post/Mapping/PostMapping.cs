using FluentNHibernate.Mapping;
using Post.Model;

namespace Post.Mapping
{
    public class PostMapping:ClassMap<post>
    {
        public PostMapping()
        {
            Id(x => x.Id, "Id").GeneratedBy.Identity();
            Map(x => x.Title, "Title").Length(255);
            Map(x => x.Content, "Content").Length(1001); // will be the max length in sql server 
            Map(x => x.Author, "Author");
            Map(x => x.Publish, "Publish");
        }
    }
}