using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original
{
    [ContextEngine(ContextEngine.Original)]
    public sealed class OriginalEngineResourceService : IEngineResourceService
    {
        private readonly IResource _resource;

        public OriginalEngineResourceService(IAssemblyResourceService assemblyResourceService)
        {
            _resource = assemblyResourceService
                .GetFromAssemblyOf<OriginalEngineResourceService>();
        }

        public byte[] GetElementData() 
            => _resource.System.GetFile("elements-zzt.bin");

        public byte[] GetMemoryData() 
            => _resource.System.GetFile("memory-zzt.bin");

        public IDictionary<string, byte[]> GetStaticFiles()
            => _resource.Root
                .GetFileNames("")
                .ToDictionary(f => f, f => _resource.Root.GetFile(f));
    }
}