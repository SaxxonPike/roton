using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class CycleCommand : ICommand
    {
        private readonly IEngine _engine;

        public CycleCommand(IEngine engine)
        {
            _engine = engine;
        }

        public string Name => "CYCLE";
        
        public void Execute(IOopContext context)
        {
            var value = _engine.Parser.ReadNumber(context.Index, context);
            if (value > 0)
            {
                context.Actor.Cycle = value;
            }
        }
    }
}