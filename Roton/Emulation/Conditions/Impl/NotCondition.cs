using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl
{
    [Context(Context.Original, "NOT")]
    [Context(Context.Super, "NOT")]
    public sealed class NotCondition : ICondition
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public NotCondition(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public bool? Execute(IOopContext context)
        {
            return !Engine.Parser.GetCondition(context);
        }
    }
}