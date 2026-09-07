using System;

namespace Roton.Emulation.Core;

public interface IEngineResourceService
{
    ReadOnlySpan<byte> GetMemoryData();
}