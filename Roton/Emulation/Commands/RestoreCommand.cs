using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class RestoreCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly IBroadcaster _broadcaster;
        private readonly IActors _actors;

        public RestoreCommand(IParser parser, IBroadcaster broadcaster, IActors actors)
        {
            _parser = parser;
            _broadcaster = broadcaster;
            _actors = actors;
        }
        
        public string Name => "RESTORE";
        
        public void Execute(IOopContext context)
        {
            _parser.ReadWord(context.Index, context);
            while (true)
            {
                context.SearchTarget = _parser.ReadWord(context.Index, context);
                var result = _broadcaster.ExecuteLabel(context.Index, context, "\xD\x27");
                if (!result)
                    break;

                while (context.SearchOffset >= 0)
                {
                    _actors[context.SearchIndex].Code[context.SearchOffset + 1] = 0x3A;
                    context.SearchOffset = _parser.Search(context.SearchIndex,
                        $"\xD\x27{_parser.ReadWord(context.Index, context)}");
                }
            }
        }
    }
}