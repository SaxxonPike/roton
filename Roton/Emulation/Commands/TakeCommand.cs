using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Zzt, "TAKE")]
    [ContextEngine(ContextEngine.SuperZzt, "TAKE")]
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