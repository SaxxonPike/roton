using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "RESTART")]
[Context(Context.Super, "RESTART")]
public sealed class RestartCommand : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        instruction = 0;
        context.NextLine = false;
    }
}