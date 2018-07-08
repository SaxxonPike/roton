using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class SetCommand : ICommand
    {
        private readonly IEngine _engine;

        public SetCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "SET";
        
        public void Execute(IOopContext context)
        {
            var flag = _engine.Parser.ReadWord(context.Index, context);
            _engine.Flags.Add(flag);
        }
    }
}