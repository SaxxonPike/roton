namespace Roton.Core
{
    public interface IRandomizerService
    {
        int Random(int exclusiveMax);
        int SyncRandom(int exclusiveMax);
    }
}