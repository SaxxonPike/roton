using System;
using System.Diagnostics;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class ComposerResourceService(
    IAssemblyResourceService assemblyResourceService)
    : IComposerResourceService
{
    private const string PaletteDataFileName = "palette.bin";
    private const string FontDataFileName = "font.bin";

    private IResource Resource { get; } = assemblyResourceService.GetFromAssemblyOf<IEngine>();

    public byte[]? GetPaletteData()
        => Resource.System.GetFile(PaletteDataFileName);

    public byte[]? GetFontData()
        => Resource.System.GetFile(FontDataFileName);
}