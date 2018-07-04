using Roton.FileIo;

namespace Roton.Resources
{
    public interface IResourceArchive
    {
        byte[] GetSuperZztElementData();
        byte[] GetSuperZztMemoryData();
        byte[] GetZztElementData();
        byte[] GetZztMemoryData();
    }
}