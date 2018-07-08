using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Commands
{
    public class SendCommand : ICommand
    {
        private readonly IEngine _engine;

        public SendCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "SEND";
        
        public void Execute(IOopContext context)
        {
            var target = _engine.Parser.ReadWord(context.Index, context);
            context.NextLine = _engine.BroadcastLabel(context.Index, target, false);
        }
    }
}