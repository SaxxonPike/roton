using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "SET")]
[Context(Context.Super, "SET")]
internal sealed class SetCommand(
    IParser parser,
    IFlags flags)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var flag = parser.ReadWord(context.Index, ref instruction, buffer);
        flags.Add(flag);
    }
}