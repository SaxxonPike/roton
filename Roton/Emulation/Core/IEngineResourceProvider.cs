namespace Roton.Emulation.Core
{
    public interface IEngineResourceProvider
    {
        byte[] GetElementData();
        byte[] GetMemoryData();
    }
}