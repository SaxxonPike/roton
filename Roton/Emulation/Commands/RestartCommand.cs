using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Zzt, "RESTART")]
    [ContextEngine(ContextEngine.SuperZzt, "RESTART")]
    public sealed class RestartCommand : ICommand
    {
        public void Execute(IOopContext context)
        {
            context.Instruction = 0;
            context.NextLine = false;
        }
    }
}