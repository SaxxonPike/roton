using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Zzt, "PUT")]
    [ContextEngine(ContextEngine.SuperZzt, "PUT")]
    public sealed class PutCommand : ICommand
    {
        private readonly IEngine _engine;

        public PutCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var vector = _engine.Parser.GetDirection(context);
            var success = false;

            if (vector != null)
            {
                var kind = _engine.Parser.GetKind(context);
                if (kind != null)
                {
                    success = true;
                    
                    var target = context.Actor.Location.Sum(vector);
                    _engine.PutTile(target, vector, kind);
                }
            }

            if (!success)
                _engine.RaiseError("Bad #PUT");
        }
    }
}