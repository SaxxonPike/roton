using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl
{
    [Context(Context.Original, "BECOME")]
    [Context(Context.Super, "BECOME")]
    public sealed class BecomeCommand : ICommand
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public BecomeCommand(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var kind = Engine.Parser.GetKind(context);
            if (kind == null)
            {
                Engine.RaiseError("Bad #BECOME");
                return;
            }

            context.Died = true;
            context.DeathTile.CopyFrom(kind);
        }
    }
}