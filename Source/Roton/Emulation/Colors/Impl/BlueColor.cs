using Roton.Infrastructure;

namespace Roton.Emulation.Colors.Impl;

[Context(Context.Original, "BLUE", 1)]
[Context(Context.Super, "BLUE", 1)]
public sealed class BlueColor : IColor
{
    public string Name => "Blue";
    public int Value => 9;
}