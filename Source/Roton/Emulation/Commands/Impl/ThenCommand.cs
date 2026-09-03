using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "THEN")]
[Context(Context.Super, "THEN")]
internal sealed class ThenCommand : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        // The actual code doesn't work this way.
        // We cheat a little by not advancing the execution counter.
        context.Resume = true;
        context.CommandsExecuted--;
    }
}