﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;

namespace Roton.Infrastructure
{
    public class AssemblyResourceService : IAssemblyResourceService
    {
        private readonly IDictionary<Assembly, IResource> _cache = new Dictionary<Assembly, IResource>();
        
        public IResource GetFromAssemblyOf<T>()
        {
            var assembly = typeof(T).Assembly;

            if (_cache.ContainsKey(assembly))
                return _cache[assembly];
            
            var name = $"{assembly.GetName().Name}.Resources.resources.zip";
            using (var stream = assembly.GetManifestResourceStream(name))
            using (var mem = new MemoryStream())
            {
                stream.CopyTo(mem);
                
                var resource = new Resource(mem.ToArray());
                _cache[assembly] = resource;
                
                return resource;
            }
        }
    }
}