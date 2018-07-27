using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl
{
    [Context(Context.Original, "CLEAR")]
    [Context(Context.Super, "CLEAR")]
    public sealed class ClearCommand : ICommand
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public ClearCommand(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var flag = Engine.Parser.ReadWord(context.Index, context);
            Engine.World.Flags.Remove(flag);
        }
    }
}