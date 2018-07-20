using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "BECOME")]
    [ContextEngine(ContextEngine.Super, "BECOME")]
    public sealed class BecomeCommand : ICommand
    {
        private readonly IEngine _engine;

        public BecomeCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var kind = _engine.Parser.GetKind(context);
            if (kind == null)
            {
                _engine.RaiseError("Bad #BECOME");
                return;
            }

            context.Died = true;
            context.DeathTile.CopyFrom(kind);
        }
    }
}