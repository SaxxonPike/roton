using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl
{
    [Context(Context.Original, "THEN")]
    [Context(Context.Super, "THEN")]
    public sealed class ThenCommand : ICommand
    {
        public void Execute(IOopContext context)
        {
            // The actual code doesn't work this way.
            // We cheat a little by not advancing the execution counter.
            context.Resume = true;
            context.CommandsExecuted--;
        }
    }
}