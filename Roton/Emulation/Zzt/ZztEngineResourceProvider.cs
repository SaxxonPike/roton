using Roton.Emulation.Core;

namespace Roton.Emulation.Zzt
{
    public class ZztEngineResourceProvider : IEngineResourceProvider
    {
        private readonly IResource _resource;

        public ZztEngineResourceProvider(IResource resource)
        {
            _resource = resource;
        }

        public byte[] GetElementData()
        {
            return _resource.System.GetFile("elements-zzt.bin");
        }

        public byte[] GetMemoryData()
        {
            return _resource.System.GetFile("memory-zzt.bin");
        }
    }
}