using Roton.Infrastructure;

namespace Roton.Emulation.Colors.Impl;

[Context(Context.Original, id: 0)]
[Context(Context.Super, id: 0)]
internal sealed class DefaultColor : IColor
{
    public string Name => string.Empty;
    public int Value => 0;
}