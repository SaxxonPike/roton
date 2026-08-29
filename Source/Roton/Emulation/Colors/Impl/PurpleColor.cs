using Roton.Infrastructure;

namespace Roton.Emulation.Colors.Impl;

[Context(Context.Original, "PURPLE", 5)]
[Context(Context.Super, "PURPLE", 5)]
internal sealed class PurpleColor : IColor
{
    public string Name => "Purple";
    public int Value => 13;
}