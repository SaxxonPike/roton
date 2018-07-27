using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl
{
    [ContextEngine(ContextEngine.Original, "PUT")]
    [ContextEngine(ContextEngine.Super, "PUT")]
    public sealed class PutCommand : ICommand
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public PutCommand(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var vector = Engine.Parser.GetDirection(context);
            var success = false;

            if (vector != null)
            {
                var kind = Engine.Parser.GetKind(context);
                if (kind != null)
                {
                    success = true;
                    
                    var target = context.Actor.Location.Sum(vector);
                    Engine.PutTile(target, vector, kind);
                }
            }

            if (!success)
                Engine.RaiseError("Bad #PUT");
        }
    }
}