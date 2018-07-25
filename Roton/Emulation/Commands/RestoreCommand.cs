using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "RESTORE")]
    [ContextEngine(ContextEngine.Super, "RESTORE")]
    public sealed class RestoreCommand : ICommand
    {
        private readonly IEngine _engine;

        public RestoreCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            _engine.Parser.ReadWord(context.Index, context);
            while (true)
            {
                context.SearchTarget = _engine.State.OopWord;
                var result = _engine.ExecuteLabel(context.Index, context, "\xD\x27");
                if (!result)
                    break;

                while (context.SearchOffset >= 0)
                {
                    _engine.Actors[context.SearchIndex].Code[context.SearchOffset + 1] = 0x3A;
                    context.SearchOffset = _engine.Parser.Search(context.SearchIndex,
                        $"\xD\x27{_engine.State.OopWord}");
                }
            }
        }
    }
}