using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class ZapCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly IBroadcaster _broadcaster;
        private readonly IActors _actors;

        public ZapCommand(IParser parser, IBroadcaster broadcaster, IActors actors)
        {
            _parser = parser;
            _broadcaster = broadcaster;
            _actors = actors;
        }
        
        public string Name => "ZAP";
        
        public void Execute(IOopContext context)
        {
            _parser.ReadWord(context.Index, context);
            while (true)
            {
                context.SearchTarget = _parser.ReadWord(context.Index, context);
                var result = _broadcaster.ExecuteLabel(context.Index, context, "\xD\x3A");
                if (!result)
                    break;
                _actors[context.SearchIndex].Code[context.SearchOffset + 1] = 0x27;
            }
        }
    }
}