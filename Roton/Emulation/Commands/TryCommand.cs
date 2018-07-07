using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Commands
{
    public class TryCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly IMover _mover;
        private readonly ITiles _tiles;

        public TryCommand(IParser parser, IMover mover, ITiles tiles)
        {
            _parser = parser;
            _mover = mover;
            _tiles = tiles;
        }
        
        public string Name => "TRY";
        
        public void Execute(IOopContext context)
        {
            var vector = _parser.GetDirection(context);
            if (vector == null)
                return;

            var target = vector.Sum(context.Actor.Location);
            if (!_tiles.ElementAt(target).IsFloor)
            {
                _mover.Push(target, vector);
            }
            if (_tiles.ElementAt(target).IsFloor)
            {
                _mover.MoveActor(context.Index, target);
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