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

    private IResource Resource =>
        _resource.Value;

    public ReadOnlySpan<byte> GetElementData() =>
        Resource.System.GetFile(elementFileName);

    public ReadOnlySpan<byte> GetMemoryData() =>
        Resource.System.GetFile(memoryFileName);

    public IDictionary<string, ReadOnlyMemory<byte>> GetStaticFiles()
        => Resource.Root
            .GetFileNames(string.Empty)
            .Select(f => (Name: f, Data: Resource.Root.GetFile(f)))
            .Where(f => f.Data != null)
            .ToDictionary(f => f.Name, f => (ReadOnlyMemory<byte>)f.Data);
}