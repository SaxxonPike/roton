namespace Roton.Emulation.Core
{
    public interface IEngineResourceService
    {
        byte[] GetElementData();
        byte[] GetMemoryData();
    }
}