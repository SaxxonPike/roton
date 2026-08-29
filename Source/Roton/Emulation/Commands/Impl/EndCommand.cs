using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "END")]
[Context(Context.Super, "END")]
internal sealed class EndCommand(
    IState state)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        state.OopByte = default;
        instruction = -1;
    }
}