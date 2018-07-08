using Roton.Core;
using Roton.Emulation.Core;
using Roton.Extensions;

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