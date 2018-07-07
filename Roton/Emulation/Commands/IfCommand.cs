using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class IfCommand : ICommand
    {
        private readonly IParser _parser;

        public IfCommand(IParser parser)
        {
            _parser = parser;
        }
        
        public string Name => "IF";
        
        public void Execute(IOopContext context)
        {
            var condition = _parser.GetCondition(context);
            if (condition.HasValue)
            {
                context.Resume = condition.Value;
            }
        }
    }
}