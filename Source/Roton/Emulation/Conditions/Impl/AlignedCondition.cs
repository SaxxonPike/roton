using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl
{
    [Context(Context.Original, "ALLIGNED")]
    [Context(Context.Super, "ALLIGNED")]
    public sealed class AlignedCondition : ICondition
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public AlignedCondition(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public bool? Execute(IOopContext context)
        {
            return context.Actor.Location.X == Engine.Player.Location.X ||
                   context.Actor.Location.Y == Engine.Player.Location.Y;
        }
    }
}