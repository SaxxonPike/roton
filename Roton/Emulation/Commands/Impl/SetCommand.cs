using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl
{
    [Context(Context.Original, "SET")]
    [Context(Context.Super, "SET")]
    public sealed class SetCommand : ICommand
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public SetCommand(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var flag = Engine.Parser.ReadWord(context.Index, context);
            Engine.World.Flags.Add(flag);
        }
    }
}