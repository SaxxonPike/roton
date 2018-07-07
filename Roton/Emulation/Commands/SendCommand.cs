using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class SendCommand : ICommand
    {
        private readonly IBroadcaster _broadcaster;
        private readonly IParser _parser;

        public SendCommand(IBroadcaster broadcaster, IParser parser)
        {
            _broadcaster = broadcaster;
            _parser = parser;
        }
        
        public string Name => "SEND";
        
        public void Execute(IOopContext context)
        {
            var target = _parser.ReadWord(context.Index, context);
            context.NextLine = _broadcaster.BroadcastLabel(context.Index, target, false);
        }
    }
}