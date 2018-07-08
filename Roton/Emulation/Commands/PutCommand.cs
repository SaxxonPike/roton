using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Commands
{
    public class PutCommand : ICommand
    {
        private readonly IEngine _engine;

        public PutCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "PUT";
        
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