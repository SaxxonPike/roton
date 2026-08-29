using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;

namespace Roton.Infrastructure.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class AssemblyResourceService : IAssemblyResourceService
{
    private readonly Dictionary<Assembly, IResource> _cache = new();
        
    public IResource GetFromAssemblyOf<T>()
    {
        var assembly = typeof(T).Assembly;

        if (_cache.TryGetValue(assembly, out var of))
            return of;
            
        var name = $"{assembly.GetName().Name}.Resources.resources.zip";
        using var stream = assembly.GetManifestResourceStream(name);
        using var mem = new MemoryStream();
        if (stream == null)
            throw new RotonException($"Reading resource failed: {name}");
                
        stream.CopyTo(mem);
                
        var resource = new Resource(mem.ToArray());
        _cache[assembly] = resource;
                
        return resource;
    }
}