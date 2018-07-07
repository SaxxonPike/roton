using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class CharCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly IEngine _engine;
        private readonly IDrawer _drawer;

        public CharCommand(IParser parser, IEngine engine, IDrawer drawer)
        {
            _parser = parser;
            _engine = engine;
            _drawer = drawer;
        }
        
        public string Name => "CHAR";
        
        public void Execute(IOopContext context)
        {
            var value = _parser.ReadNumber(context.Index, context);
            if (value >= 0)
            {
                context.Actor.P1 = value;
                _drawer.UpdateBoard(context.Actor.Location);
            }
        }
    }
}