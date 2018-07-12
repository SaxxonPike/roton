using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "GIVE")]
    [ContextEngine(ContextEngine.Super, "GIVE")]
    public sealed class GiveCommand : ICommand
    {
        private readonly IEngine _engine;

        public GiveCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            context.Resume = _engine.ExecuteTransaction(context, false);
            _engine.Hud.UpdateStatus();
        }
    }
}