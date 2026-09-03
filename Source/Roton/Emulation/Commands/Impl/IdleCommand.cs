using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "IDLE")]
[Context(Context.Super, "IDLE")]
internal sealed class IdleCommand : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction) => 
        context.Moved = true;
}