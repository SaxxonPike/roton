using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Interface.Infrastructure
{
    public sealed class InterfaceResourceService : IInterfaceResourceService
    {
        private readonly IResource _resource;

        public InterfaceResourceService(IAssemblyResourceService assemblyResourceService)
        {
            _resource = assemblyResourceService
                .GetFromAssemblyOf<InterfaceResourceService>();
        }
        
        public byte[] GetPaletteData() 
            => _resource.System.GetFile("palette.bin");

        public byte[] GetFontData() 
            => _resource.System.GetFile("font.bin");
    }
}