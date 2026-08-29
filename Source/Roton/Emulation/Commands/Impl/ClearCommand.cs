using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "CLEAR")]
[Context(Context.Super, "CLEAR")]
internal sealed class ClearCommand(
    IWorld world,
    IParser parser)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var flag = parser.ReadWord(context.Index, ref instruction, buffer);
        world.Flags.Remove(flag);
    }
}