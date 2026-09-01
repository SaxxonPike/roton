using Roton.Infrastructure;

namespace Roton.Emulation.Colors.Impl;

[Context(Context.Original, 0)]
[Context(Context.Super, 0)]
internal sealed class DefaultColor : IColor
{
    public string Name => "";
    public int Value => 0;
}