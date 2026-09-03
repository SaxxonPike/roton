using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "SEEK")]
[Context(Context.Super, "SEEK")]
internal sealed class SeekDirection(
    INavigator navigator)
    : IDirection
{
    public Vector Execute(ref OopContext context, ref Word instruction) => 
        navigator.Seek(context.Actor.Location);
}