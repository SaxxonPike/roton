using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Commands
{
    public class TryCommand : ICommand
    {
        private readonly IEngine _engine;

        public TryCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "TRY";
        
        public void Execute(IOopContext context)
        {
            var vector = _engine.Parser.GetDirection(context);
            if (vector == null)
                return;

            var target = vector.Sum(context.Actor.Location);
            if (!_engine.Tiles.ElementAt(target).IsFloor)
            {
                _engine.Push(target, vector);
            }
            if (_engine.ElementAt(target).IsFloor)
            {
                _engine.MoveActor(context.Index, target);
                context.Moved = true;
                context.Resume = false;
            }
            else
            {
                context.Resume = true;
            }
        }
    }
}