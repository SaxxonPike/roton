using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class CycleCommand : ICommand
    {
        private readonly IParser _parser;

        public CycleCommand(IParser parser)
        {
            _parser = parser;
        }

        public string Name => "CYCLE";
        
        public void Execute(IOopContext context)
        {
            var value = _parser.ReadNumber(context.Index, context);
            if (value > 0)
            {
                context.Actor.Cycle = value;
            }
        }
    }
}