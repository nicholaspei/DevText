using System.Diagnostics;

namespace DevText.Framework.Data
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        [DebuggerStepThrough]
        public IUnitOfWork GetNewUnitOfWork()
        {
            return IoC.GetInstance<IUnitOfWork>();
            //return new UnitOfWork(BootStrapperWrapper.GetInstance<ISessionFactory>());
        }
    }
}
