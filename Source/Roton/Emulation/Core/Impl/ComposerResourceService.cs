using System;
using System.Diagnostics;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class ComposerResourceService(IAssemblyResourceService assemblyResourceService) : IComposerResourceService
{
    public const string PaletteDataFileName = "palette.bin";
    public const string FontDataFileName = "font.bin";
        
    private readonly Lazy<IResource> _resource = new(assemblyResourceService.GetFromAssemblyOf<IEngine>);

    private IResource Resource
    {
        [DebuggerStepThrough] get => _resource.Value;
    }

    public byte[]? GetPaletteData() 
        => Resource.System.GetFile(PaletteDataFileName);

    public byte[]? GetFontData() 
        => Resource.System.GetFile(FontDataFileName);

}