using Roton.Infrastructure;

namespace Roton.Emulation.Colors.Impl;

[Context(Context.Original, "WHITE", 7)]
[Context(Context.Super, "WHITE", 7)]
public sealed class WhiteColor : IColor
{
    public string Name => "White";
    public int Value => 15;
}