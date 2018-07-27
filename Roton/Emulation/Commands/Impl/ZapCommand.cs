using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl
{
    [Context(Context.Original, "ZAP")]
    [Context(Context.Super, "ZAP")]
    public sealed class ZapCommand : ICommand
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public ZapCommand(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            Engine.Parser.ReadWord(context.Index, context);
            while (true)
            {
                context.SearchTarget = Engine.State.OopWord;
                var result = Engine.ExecuteLabel(context.Index, context, "\xD\x3A");
                if (!result)
                    break;
                Engine.Actors[context.SearchIndex].Code[context.SearchOffset + 1] = 0x27;
            }
        }
    }
}