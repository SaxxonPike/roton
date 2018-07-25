using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "DIE")]
    [ContextEngine(ContextEngine.Super, "DIE")]
    public sealed class DieCommand : ICommand
    {
        private readonly IEngine _engine;

        public DieCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            context.Died = true;
            context.DeathTile.SetTo(_engine.ElementList.EmptyId, 0);
        }
    }
}