using System.Collections.Generic;

namespace Roton.Resources
{
    public interface IResourceArchive
    {
        byte[] GetZztElementData();
        byte[] GetSuperZztElementData();
        byte[] GetZztMemoryData();
        byte[] GetSuperZztMemoryData();
        IEnumerable<string> GetRootFileNames();
    }
}