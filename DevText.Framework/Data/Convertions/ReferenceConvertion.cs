using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace DevText.Framework.Data.Convertion
{
    public class ReferenceConvertion:IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            instance.Column(instance.Property.Name + "Id");
        }
    }
}
