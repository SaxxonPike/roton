using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "RESTART")]
[Context(Context.Super, "RESTART")]
internal sealed class RestartCommand : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        instruction = 0;
        context.NextLine = false;
    }
}