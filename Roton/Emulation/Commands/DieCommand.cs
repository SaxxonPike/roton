using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Commands
{
    public class DieCommand : ICommand
    {
        private readonly IEngine _engine;

        public DieCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "DIE";
        
        public void Execute(IOopContext context)
        {
            context.Died = true;
            context.DeathTile.SetTo(_engine.Elements.EmptyId, 0);
        }
    }
}