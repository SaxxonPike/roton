using System;
using System.Collections.Generic;

namespace Roton.Emulation.Core;

public interface IEngineResourceService
{
    ReadOnlySpan<byte> GetElementData();
    ReadOnlySpan<byte> GetMemoryData();
    IDictionary<string, ReadOnlyMemory<byte>> GetStaticFiles();
}