using Roton.Core;
using Roton.Resources;

namespace Roton.Emulation.ZZT
{
    public class ZztStaticResourceService : IStaticResourceService
    {
        private readonly IResourceArchive _resourceArchive;

        public ZztStaticResourceService(IResourceArchive resourceArchive)
        {
            _resourceArchive = resourceArchive;
        }

        public byte[] GetElementData()
        {
            return _resourceArchive.GetZztElementData();
        }

        public byte[] GetMemoryData()
        {
            return _resourceArchive.GetZztMemoryData();
        }
    }
}