using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "UNLOCK")]
[Context(Context.Super, "UNLOCK")]
public sealed class UnlockCommand(
    IActorLocker actorLocker) 
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        actorLocker.UnlockActor(context.Index);
    }
}