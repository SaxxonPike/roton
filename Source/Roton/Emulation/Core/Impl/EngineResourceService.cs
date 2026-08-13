using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

public abstract class EngineResourceService(
    IAssemblyResourceService assemblyResourceService,
    string elementFileName,
    string memoryFileName)
    : IEngineResourceService
{
    private readonly Lazy<IResource> _resource = new(assemblyResourceService.GetFromAssemblyOf<IEngine>);

    private IResource Resource => _resource.Value;

    public byte[] GetElementData() => Resource.System.GetFile(elementFileName);

    public byte[] GetMemoryData() => Resource.System.GetFile(memoryFileName);

    public IDictionary<string, byte[]> GetStaticFiles()
        => Resource.Root
            .GetFileNames(string.Empty)
            .ToDictionary(f => f, f => Resource.Root.GetFile(f));
}