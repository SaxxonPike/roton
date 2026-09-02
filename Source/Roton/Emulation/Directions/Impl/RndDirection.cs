using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "RND")]
[Context(Context.Super, "RND")]
internal sealed class RndDirection(
    INavigator navigator) 
    : IDirection
{
    public Vector Execute(ref OopContext context, ref Word instruction) => 
        navigator.Rnd();
}