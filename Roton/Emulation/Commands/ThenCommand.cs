using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "THEN")]
    [ContextEngine(ContextEngine.Super, "THEN")]
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