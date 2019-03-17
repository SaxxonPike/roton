using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl
{
    [Context(Context.Original, "BLOCKED")]
    [Context(Context.Super, "BLOCKED")]
    public sealed class BlockedCondition : ICondition
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public BlockedCondition(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public bool? Execute(IOopContext context)
        {
            var direction = Engine.Parser.GetDirection(context);
            if (direction == null)
                return null;

            return !Engine.ElementAt(context.Actor.Location.Sum(direction)).IsFloor;
        }
    }
}