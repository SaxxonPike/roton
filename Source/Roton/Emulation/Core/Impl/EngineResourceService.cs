using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

public abstract class EngineResourceService : IEngineResourceService
{
    private readonly string _elementFileName;
    private readonly string _memoryFileName;
    private readonly Lazy<IResource> _resource;

    protected EngineResourceService(
        IAssemblyResourceService assemblyResourceService,
        string elementFileName,
        string memoryFileName)
    {
        _elementFileName = elementFileName;
        _memoryFileName = memoryFileName;
        _resource = new Lazy<IResource>(assemblyResourceService.GetFromAssemblyOf<IEngine>);
    }

    private IResource Resource => _resource.Value;

    public byte[] GetElementData() => Resource.System.GetFile(_elementFileName);

    public byte[] GetMemoryData() => Resource.System.GetFile(_memoryFileName);

    public IDictionary<string, byte[]> GetStaticFiles()
        => Resource.Root
            .GetFileNames(string.Empty)
            .ToDictionary(f => f, f => Resource.Root.GetFile(f));
}