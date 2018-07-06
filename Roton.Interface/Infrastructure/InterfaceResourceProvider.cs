using Roton.Core;

namespace Roton.Interface.Infrastructure
{
    public class InterfaceResourceProvider : IInterfaceResourceProvider
    {
        private readonly IResource _resource;

        public InterfaceResourceProvider(IResource resource)
        {
            _resource = resource;
        }
        
        public byte[] GetPaletteData()
        {
            return _resource.System.GetFile("palette.bin");
        }

        public byte[] GetFontData()
        {
            return _resource.System.GetFile("font.bin");
        }
    }
}