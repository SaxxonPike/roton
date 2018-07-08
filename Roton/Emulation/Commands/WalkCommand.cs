using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Commands
{
    public class WalkCommand : ICommand
    {
        private readonly IEngine _engine;

        public WalkCommand(IEngine engine)
        {
            _engine = engine;
        }

        public string Name => "WALK";
        
        public void Execute(IOopContext context)
        {
            var vector = _engine.Parser.GetDirection(context);
            if (vector != null)
            {
                context.Actor.Vector.CopyFrom(vector);
            }
        }
    }
}