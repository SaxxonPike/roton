using System.Collections.Generic;

namespace Roton.Emulation.Core;

public interface IEngineResourceService
{
    byte[] GetElementData();
    byte[] GetMemoryData();
    IDictionary<string, byte[]> GetStaticFiles();
}