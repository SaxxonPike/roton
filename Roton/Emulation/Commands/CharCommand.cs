using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "CHAR")]
    [ContextEngine(ContextEngine.Super, "CHAR")]
    public sealed class CharCommand : ICommand
    {
        private readonly IEngine _engine;

        public CharCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var value = _engine.Parser.ReadNumber(context.Index, context);
            if (value >= 0)
            {
                context.Actor.P1 = value;
                _engine.UpdateBoard(context.Actor.Location);
            }
        }
    }
}