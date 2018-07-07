using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Commands
{
    public class GoCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly ITiles _tiles;
        private readonly IMover _mover;

        public GoCommand(IParser parser, ITiles tiles, IMover mover)
        {
            _parser = parser;
            _tiles = tiles;
            _mover = mover;
        }
        
        public string Name => "GO";
        
        public void Execute(IOopContext context)
        {
            var vector = _parser.GetDirection(context);
            if (vector != null)
            {
                var target = context.Actor.Location.Sum(vector);
                if (!_tiles.ElementAt(target).IsFloor)
                {
                    _mover.Push(target, vector);
                }
                if (_tiles.ElementAt(target).IsFloor)
                {
                    _mover.MoveActor(context.Index, target);
                    context.Moved = true;
                }
                else
                {
                    context.Repeat = true;
                }
            }
        }
    }
}