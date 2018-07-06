using Roton.Core;

namespace Roton.Emulation.SuperZZT
{
    public class SuperZztEngineResourceProvider : IEngineResourceProvider
    {
        private readonly IResource _resource;

        public SuperZztEngineResourceProvider(IResource resource)
        {
            _resource = resource;
        }

        public byte[] GetElementData()
        {
            return _resource.System.GetFile("elements-szzt.bin");
        }

        public byte[] GetMemoryData()
        {
            return _resource.System.GetFile("memory-szzt.bin");
        }
    }
}