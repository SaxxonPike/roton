using Roton.Infrastructure;

namespace Roton.Emulation.Colors.Impl;

[Context(Context.Original, "RED", 4)]
[Context(Context.Super, "RED", 4)]
public sealed class RedColor : IColor
{
    public string Name => "Red";
    public int Value => 12;
}