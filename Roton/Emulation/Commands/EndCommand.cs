using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Commands
{
    public class EndCommand : ICommand
    {
        private readonly IEngine _engine;

        public EndCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "END";
        
        public void Execute(IOopContext context)
        {
            _engine.State.OopByte = 0;
            context.Instruction = -1;
        }
    }
}