using System;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

public abstract class EngineResourceService(
    IAssemblyResourceService assemblyResourceService,
    string elementFileName,
    string memoryFileName)
    : IEngineResourceService
{
    private IResource Resource { get; } = assemblyResourceService.GetFromAssemblyOf<IGame>();

    public ReadOnlySpan<byte> GetElementData() =>
        Resource.System.GetFile(elementFileName);

    public ReadOnlySpan<byte> GetMemoryData() =>
        Resource.System.GetFile(memoryFileName);
}