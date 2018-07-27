using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl
{
    [ContextEngine(ContextEngine.Original, "CYCLE")]
    [ContextEngine(ContextEngine.Super, "CYCLE")]
    public sealed class CycleCommand : ICommand
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public CycleCommand(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var value = Engine.Parser.ReadNumber(context.Index, context);
            if (value > 0)
            {
                context.Actor.Cycle = value;
            }
        }
    }
}