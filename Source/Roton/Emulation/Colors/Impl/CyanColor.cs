using Roton.Infrastructure;

namespace Roton.Emulation.Colors.Impl;

[Context(Context.Original, "CYAN", 3)]
[Context(Context.Super, "CYAN", 3)]
internal sealed class CyanColor : IColor
{
    public string Name => "Cyan";
    public int Value => 11;
}