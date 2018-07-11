using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.SuperZzt
{
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class SuperZztEngineResourceService : IEngineResourceService
    {
        private readonly IResource _resource;

        public SuperZztEngineResourceService(IAssemblyResourceService assemblyResourceService)
        {
            _resource = assemblyResourceService
                .GetFromAssemblyOf<SuperZztEngineResourceService>();
        }

        public byte[] GetElementData() 
            => _resource.System.GetFile("elements-szzt.bin");

        public byte[] GetMemoryData() 
            => _resource.System.GetFile("memory-szzt.bin");
    }
}