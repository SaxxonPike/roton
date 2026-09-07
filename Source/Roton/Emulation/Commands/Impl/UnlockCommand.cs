using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "UNLOCK")]
[Context(Context.Super, "UNLOCK")]
internal sealed class UnlockCommand(
    IActorLocker actorLocker) 
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        actorLocker.Unlock(context.Index);
    }
}