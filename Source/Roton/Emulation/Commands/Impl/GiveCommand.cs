using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "GIVE")]
[Context(Context.Super, "GIVE")]
internal sealed class GiveCommand(
    IHud hud,
    ITransactor transactor)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        context.Resume = transactor.Execute(ref context, ref instruction, false);
        hud.UpdateStatus();
    }
}