using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class StarBehavior : EnemyBehavior
    {
        private readonly IEngine _engine;
        private readonly IActors _actors;
        private readonly IGrid _grid;
        private readonly IElements _elements;
        private readonly IState _state;

        public override string KnownName => KnownNames.Star;

        public StarBehavior(IEngine engine, IActors actors, IGrid grid, IElements elements, IState state) : base(engine)
        {
            _engine = engine;
            _actors = actors;
            _grid = grid;
            _elements = elements;
            _state = state;
        }
        
        public override void Act(int index)
        {
            var actor = _actors[index];

            actor.P2 = (actor.P2 - 1) & 0xFF;
            if (actor.P2 > 0)
            {
                if ((actor.P2 & 1) == 0)
                {
                    actor.Vector.CopyFrom(_engine.Seek(actor.Location));
                    var targetLocation = actor.Location.Sum(actor.Vector);
                    var targetElement = _grid.ElementAt(targetLocation);

                    if (targetElement.Id == _elements.PlayerId || targetElement.Id == _elements.BreakableId)
                    {
                        _engine.Attack(index, targetLocation);
                    }
                    else
                    {
                        if (!targetElement.IsFloor)
                        {
                            _engine.Push(targetLocation, actor.Vector);
                        }
                        if (targetElement.IsFloor || targetElement.Id == _elements.WaterId)
                        {
                            _engine.MoveActor(index, targetLocation);
                        }
                    }
                }
                else
                {
                    _engine.UpdateBoard(actor.Location);
                }
            }
            else
            {
                _engine.RemoveActor(index);
            }
        }

        public override AnsiChar Draw(IXyPair location)
        {
            var tile = _grid[location];
            tile.Color++;
            if (tile.Color > 15)
                tile.Color = 9;
            return new AnsiChar(_state.StarChars[_state.GameCycle & 0x3], tile.Color);
        }
    }
}