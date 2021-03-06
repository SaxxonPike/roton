using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl
{
    [Context(Context.Original, "RESTORE")]
    [Context(Context.Super, "RESTORE")]
    public sealed class RestoreCommand : ICommand
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public RestoreCommand(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            Engine.Parser.ReadWord(context.Index, context);
            context.SearchIndex = 0;
            while (true)
            {
                context.SearchTarget = Engine.State.OopWord;
                var result = Engine.ExecuteLabel(context.Index, context, "\xD\x27");
                if (!result)
                    break;

                while (context.SearchOffset >= 0)
                {
                    Engine.Actors[context.SearchIndex].Code[context.SearchOffset + 1] = 0x3A;
                    context.SearchOffset = Engine.Parser.Search(context.SearchIndex, 0,
                        $"\xD\x27{Engine.State.OopWord}");
                }
            }
        }
    }
}