using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "CLEAR")]
[Context(Context.Super, "CLEAR")]
public sealed class ClearCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var flag = Engine.Parser.ReadWord(context.Index, ref instruction, buffer);
        Engine.World.Flags.Remove(flag);
    }
}