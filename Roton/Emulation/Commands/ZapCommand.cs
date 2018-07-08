using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class ZapCommand : ICommand
    {
        private readonly IEngine _engine;

        public ZapCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "ZAP";
        
        public void Execute(IOopContext context)
        {
            _engine.Parser.ReadWord(context.Index, context);
            while (true)
            {
                context.SearchTarget = _engine.Parser.ReadWord(context.Index, context);
                var result = _engine.ExecuteLabel(context.Index, context, "\xD\x3A");
                if (!result)
                    break;
                _engine.Actors[context.SearchIndex].Code[context.SearchOffset + 1] = 0x27;
            }
        }
    }
}