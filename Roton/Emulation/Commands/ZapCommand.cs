using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Zzt, "ZAP")]
    [ContextEngine(ContextEngine.SuperZzt, "ZAP")]
    public sealed class ZapCommand : ICommand
    {
        private readonly IEngine _engine;

        public ZapCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            _engine.Parser.ReadWord(context.Index, context);
            while (true)
            {
                context.SearchTarget = _engine.State.OopWord;
                var result = _engine.ExecuteLabel(context.Index, context, "\xD\x3A");
                if (!result)
                    break;
                _engine.Actors[context.SearchIndex].Code[context.SearchOffset + 1] = 0x27;
            }
        }
    }
}