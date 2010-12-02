using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace DevText.Framework.Data.Convertion
{
    public class PrimaryKeyNameConvertion:IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.Column(instance.EntityType.Name + "Id");
        }
    }
}
