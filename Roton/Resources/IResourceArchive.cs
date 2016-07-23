using System.Collections.Generic;

namespace Roton.Resources
{
    public interface IResourceArchive
    {
        IEnumerable<string> GetRootFileNames();
        byte[] GetSuperZztElementData();
        byte[] GetSuperZztMemoryData();
        byte[] GetZztElementData();
        byte[] GetZztMemoryData();
    }
}