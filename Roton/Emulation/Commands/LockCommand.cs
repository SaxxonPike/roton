using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Commands
{
    public class LockCommand : ICommand
    {
        private readonly IEngine _engine;

        public LockCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "LOCK";
        
        public void Execute(IOopContext context)
        {
            _engine.LockActor(context.Index);
        }
    }
}