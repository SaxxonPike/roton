using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Commands
{
    public class IfCommand : ICommand
    {
        private readonly IEngine _engine;

        public IfCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "IF";
        
        public void Execute(IOopContext context)
        {
            var condition = _engine.Parser.GetCondition(context);
            
            if (condition.HasValue)
                context.Resume = condition.Value;
        }
    }
}