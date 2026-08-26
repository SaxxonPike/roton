using Roton.Infrastructure;

namespace Roton.Emulation.Colors.Impl;

[Context(Context.Original, "YELLOW", 6)]
[Context(Context.Super, "YELLOW", 6)]
public sealed class YellowColor : IColor
{
    public string Name => "Yellow";
    public int Value => 14;
}