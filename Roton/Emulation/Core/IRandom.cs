namespace Roton.Core
{
    public interface IRandom
    {
        int NonSynced(int exclusiveMax);
        int Synced(int exclusiveMax);
    }
}