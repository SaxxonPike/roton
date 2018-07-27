using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl
{
    [Context(Context.Original, "GIVE")]
    [Context(Context.Super, "GIVE")]
    public sealed class GiveCommand : ICommand
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public GiveCommand(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            context.Resume = Engine.ExecuteTransaction(context, false);
            Engine.Hud.UpdateStatus();
        }
    }
}