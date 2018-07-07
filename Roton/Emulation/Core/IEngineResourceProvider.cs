namespace Roton.Core
{
    public interface IEngineResourceProvider
    {
        byte[] GetElementData();
        byte[] GetMemoryData();
    }
}