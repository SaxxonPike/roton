using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "END")]
    [ContextEngine(ContextEngine.Super, "END")]
    public sealed class EndCommand : ICommand
    {
        private readonly IEngine _engine;

        public EndCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            _engine.State.OopByte = 0;
            context.Instruction = -1;
        }
    }
}