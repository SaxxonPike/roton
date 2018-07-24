using System;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class ComposerResourceService : IComposerResourceService
    {
        public const string PaletteDataFileName = "palette.bin";
        public const string FontDataFileName = "font.bin";
        
        private readonly Lazy<IResource> _resource;

        public ComposerResourceService(IAssemblyResourceService assemblyResourceService)
        {
            _resource = new Lazy<IResource>(assemblyResourceService.GetFromAssemblyOf<IEngine>);
        }

        private IResource Resource => _resource.Value;
        
        public byte[] GetPaletteData() 
            => Resource.System.GetFile(PaletteDataFileName);

        public byte[] GetFontData() 
            => Resource.System.GetFile(FontDataFileName);

    }
}