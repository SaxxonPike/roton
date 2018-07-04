using Roton.Core;
using Roton.Resources;

namespace Roton.Emulation.SuperZZT
{
    public class SuperZztStaticResourceService : IStaticResourceService
    {
        private readonly IResourceArchive _resourceArchive;

        public SuperZztStaticResourceService(IResourceArchive resourceArchive)
        {
            _resourceArchive = resourceArchive;
        }

        public byte[] GetElementData()
        {
            return _resourceArchive.GetSuperZztElementData();
        }

        public byte[] GetMemoryData()
        {
            return _resourceArchive.GetSuperZztMemoryData();
        }
    }
}