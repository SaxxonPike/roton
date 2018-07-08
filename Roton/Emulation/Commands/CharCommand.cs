using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class CharCommand : ICommand
    {
        private readonly IEngine _engine;

        public CharCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "CHAR";
        
        public void Execute(IOopContext context)
        {
            var value = _engine.Parser.ReadNumber(context.Index, context);
            if (value >= 0)
            {
                context.Actor.P1 = value;
                _engine.UpdateBoard(context.Actor.Location);
            }
        }
    }
}