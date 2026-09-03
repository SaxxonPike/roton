using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "LOCK")]
[Context(Context.Super, "LOCK")]
internal sealed class LockCommand(
    IActorLocker actorLocker)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        actorLocker.LockActor(context.Index);
    }
}