using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace DevText.Framework.Data.Convertion
{
    public class ForeignKeyNameConvention:IHasManyConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Key.Column(instance.EntityType.Name + "Id");
        }
    }
}
