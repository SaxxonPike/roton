using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class RestoreCommand : ICommand
    {
        private readonly IEngine _engine;

        public RestoreCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "RESTORE";
        
        public void Execute(IOopContext context)
        {
            _engine.Parser.ReadWord(context.Index, context);
            while (true)
            {
                context.SearchTarget = _engine.Parser.ReadWord(context.Index, context);
                var result = _engine.ExecuteLabel(context.Index, context, "\xD\x27");
                if (!result)
                    break;

                while (context.SearchOffset >= 0)
                {
                    _engine.Actors[context.SearchIndex].Code[context.SearchOffset + 1] = 0x3A;
                    context.SearchOffset = _engine.Parser.Search(context.SearchIndex,
                        $"\xD\x27{_engine.Parser.ReadWord(context.Index, context)}");
                }
            }
        }
    }
}