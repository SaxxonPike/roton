using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl
{
    [Context(Context.Original, "ANY")]
    [Context(Context.Super, "ANY")]
    public sealed class AnyCondition : ICondition
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public AnyCondition(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public bool? Execute(IOopContext context)
        {
            var kind = Engine.Parser.GetKind(context);
            if (kind == null)
                return null;

            return Engine.FindTile(kind, new Location(0, 1));
        }
    }
}