namespace DevText.Framework.Data
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork GetNewUnitOfWork();
    }
}
