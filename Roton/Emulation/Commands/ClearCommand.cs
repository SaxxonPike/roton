using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class ClearCommand : ICommand
    {
        private readonly IEngine _engine;

        public ClearCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "CLEAR";
        
        public void Execute(IOopContext context)
        {
            var flag = _engine.Parser.ReadWord(context.Index, context);
            _engine.Flags.Remove(flag);
        }
    }
}