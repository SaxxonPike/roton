using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super
{
    [ContextEngine(ContextEngine.Super)]
    public sealed class SuperEngineResourceService : IEngineResourceService
    {
        private readonly IResource _resource;

        public SuperEngineResourceService(IAssemblyResourceService assemblyResourceService)
        {
            _resource = assemblyResourceService
                .GetFromAssemblyOf<SuperEngineResourceService>();
        }

        public byte[] GetElementData() 
            => _resource.System.GetFile("elements-szzt.bin");

        public byte[] GetMemoryData() 
            => _resource.System.GetFile("memory-szzt.bin");
        
        public byte[] GetPaletteData() 
            => _resource.System.GetFile("palette.bin");

        public byte[] GetFontData() 
            => _resource.System.GetFile("font.bin");

        public IDictionary<string, byte[]> GetStaticFiles()
            => _resource.Root
                .GetFileNames("")
                .ToDictionary(f => f, f => _resource.Root.GetFile(f));
    }
}