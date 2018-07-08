using Roton.Emulation.Core;
using Roton.Emulation.Data;

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