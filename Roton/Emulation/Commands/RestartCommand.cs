using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "RESTART")]
    [ContextEngine(ContextEngine.Super, "RESTART")]
    public sealed class RestartCommand : ICommand
    {
        public void Execute(IOopContext context)
        {
            context.Instruction = 0;
            context.NextLine = false;
        }
    }
}