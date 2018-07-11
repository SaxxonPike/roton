using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Zzt, "SEND")]
    [ContextEngine(ContextEngine.SuperZzt, "SEND")]
    public sealed class SendCommand : ICommand
    {
        private readonly IEngine _engine;

        public SendCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var target = _engine.Parser.ReadWord(context.Index, context);
            context.NextLine = _engine.BroadcastLabel(context.Index, target, false);
        }
    }
}