using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "IDLE")]
    [ContextEngine(ContextEngine.Super, "IDLE")]
    public sealed class IdleCommand : ICommand
    {
        public void Execute(IOopContext context)
        {
            context.Moved = true;
        }
    }
}