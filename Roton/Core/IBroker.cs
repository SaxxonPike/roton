namespace Roton.Core
{
    public interface IBroker
    {
        bool ExecuteTransaction(IOopContext context, bool take);
    }
}