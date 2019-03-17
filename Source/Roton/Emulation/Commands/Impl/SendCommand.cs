using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl
{
    [Context(Context.Original, "SEND")]
    [Context(Context.Super, "SEND")]
    public sealed class SendCommand : ICommand
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public SendCommand(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var target = Engine.Parser.ReadWord(context.Index, context);
            context.NextLine = Engine.BroadcastLabel(context.Index, target, false);
        }
    }
}