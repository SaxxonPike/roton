using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "CYCLE")]
    [ContextEngine(ContextEngine.Super, "CYCLE")]
    public sealed class CycleCommand : ICommand
    {
        private readonly IEngine _engine;

        public CycleCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var value = _engine.Parser.ReadNumber(context.Index, context);
            if (value > 0)
            {
                context.Actor.Cycle = value;
            }
        }
    }
}