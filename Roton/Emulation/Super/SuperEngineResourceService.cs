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
    }
}