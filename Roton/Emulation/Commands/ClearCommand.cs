using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "CLEAR")]
    [ContextEngine(ContextEngine.Super, "CLEAR")]
    public sealed class ClearCommand : ICommand
    {
        private readonly IEngine _engine;

        public ClearCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var flag = _engine.Parser.ReadWord(context.Index, context);
            _engine.World.Flags.Remove(flag);
        }
    }
}