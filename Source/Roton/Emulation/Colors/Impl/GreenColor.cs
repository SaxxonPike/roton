using Roton.Infrastructure;

namespace Roton.Emulation.Colors.Impl;

[Context(Context.Original, "GREEN", 2)]
[Context(Context.Super, "GREEN", 2)]
public sealed class GreenColor : IColor
{
    public string Name => "Green";
    public int Value => 10;
}