using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class StarBehavior : EnemyBehavior
    {
        private readonly IActors _actors;
        private readonly ITiles _tiles;
        private readonly IElements _elements;
        private readonly IState _state;
        private readonly ICompass _compass;
        private readonly IMover _mover;
        private readonly IDrawer _drawer;

        public override string KnownName => KnownNames.Star;

        public StarBehavior(IActors actors, ITiles tiles, IElements elements, IState state,
            ICompass compass, IMover mover, IDrawer drawer) : base(mover)
        {
            _actors = actors;
            _tiles = tiles;
            _elements = elements;
            _state = state;
            _compass = compass;
            _mover = mover;
            _drawer = drawer;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];

            actor.P2 = (actor.P2 - 1) & 0xFF;
            if (actor.P2 > 0)
            {
                if ((actor.P2 & 1) == 0)
                {
                    actor.Vector.CopyFrom(_compass.Seek(actor.Location));
                    var targetLocation = actor.Location.Sum(actor.Vector);
                    var targetElement = _tiles.ElementAt(targetLocation);

                    if (targetElement.Id == _elements.PlayerId || targetElement.Id == _elements.BreakableId)
                    {
                        _mover.Attack(index, targetLocation);
                    }
                    else
                    {
                        if (!targetElement.IsFloor)
                        {
                            _mover.Push(targetLocation, actor.Vector);
                        }

                        if (targetElement.IsFloor || targetElement.Id == _elements.WaterId)
                        {
                            _mover.MoveActor(index, targetLocation);
                        }
                    }
                }
                else
                {
                    _drawer.UpdateBoard(actor.Location);
                }
            }
            else
            {
                _mover.RemoveActor(index);
            }
        }

        public override AnsiChar Draw(IXyPair location)
        {
            var tile = _tiles[location];
            tile.Color++;
            if (tile.Color > 15)
                tile.Color = 9;
            return new AnsiChar(_state.StarChars[_state.GameCycle & 0x3], tile.Color);
        }
    }
}