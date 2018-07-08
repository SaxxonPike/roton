using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Commands
{
    public class UnlockCommand : ICommand
    {
        private readonly IEngine _engine;

        public UnlockCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "UNLOCK";
        
        public void Execute(IOopContext context)
        {
            _engine.UnlockActor(context.Index);
        }
    }
}