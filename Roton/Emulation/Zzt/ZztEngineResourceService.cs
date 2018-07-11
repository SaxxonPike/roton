using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Zzt
{
    [ContextEngine(ContextEngine.Zzt)]
    public sealed class ZztEngineResourceService : IEngineResourceService
    {
        private readonly IResource _resource;

        public ZztEngineResourceService(IAssemblyResourceService assemblyResourceService)
        {
            _resource = assemblyResourceService
                .GetFromAssemblyOf<ZztEngineResourceService>();
        }

        public byte[] GetElementData() 
            => _resource.System.GetFile("elements-zzt.bin");

        public byte[] GetMemoryData() 
            => _resource.System.GetFile("memory-zzt.bin");
    }
}