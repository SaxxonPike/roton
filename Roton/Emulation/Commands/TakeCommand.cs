using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "TAKE")]
    [ContextEngine(ContextEngine.Super, "TAKE")]
    public sealed class TakeCommand : ICommand
    {
        private readonly IEngine _engine;

        public TakeCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            context.Resume = _engine.ExecuteTransaction(context, true);
            _engine.Hud.UpdateStatus();
        }
    }
}