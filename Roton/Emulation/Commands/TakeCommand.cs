using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Commands
{
    public class TakeCommand : ICommand
    {
        private readonly IEngine _engine;

        public TakeCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "TAKE";
        
        public void Execute(IOopContext context)
        {
            context.Resume = _engine.ExecuteTransaction(context, true);
            _engine.Hud.UpdateStatus();
        }
    }
}