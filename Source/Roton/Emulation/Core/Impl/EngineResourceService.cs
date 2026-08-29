using System;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

public abstract class EngineResourceService(
    IAssemblyResourceService assemblyResourceService,
    string elementFileName,
    string memoryFileName)
    : IEngineResourceService
{
    private readonly Lazy<IResource> _resource = new(assemblyResourceService.GetFromAssemblyOf<IEngine>);

    private IResource Resource =>
        _resource.Value;

    public ReadOnlySpan<byte> GetElementData() =>
        Resource.System.GetFile(elementFileName);

    public ReadOnlySpan<byte> GetMemoryData() =>
        Resource.System.GetFile(memoryFileName);
}