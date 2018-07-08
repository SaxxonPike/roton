using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Behaviors
{
    public sealed class StarBehavior : EnemyBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Star;

        public StarBehavior(IEngine engine) : base(engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            var actor = _engine.Actors[index];

            actor.P2 = (actor.P2 - 1) & 0xFF;
            if (actor.P2 > 0)
            {
                if ((actor.P2 & 1) == 0)
                {
                    actor.Vector.CopyFrom(_engine.Seek(actor.Location));
                    var targetLocation = actor.Location.Sum(actor.Vector);
                    var targetElement = _engine.Tiles.ElementAt(targetLocation);

                    if (targetElement.Id == _engine.Elements.PlayerId || targetElement.Id == _engine.Elements.BreakableId)
                    {
                        _engine.Attack(index, targetLocation);
                    }
                    else
                    {
                        if (!targetElement.IsFloor)
                        {
                            _engine.Push(targetLocation, actor.Vector);
                        }

                        if (targetElement.IsFloor || targetElement.Id == _engine.Elements.WaterId)
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
            var tile = _engine.Tiles[location];
            tile.Color++;
            if (tile.Color > 15)
                tile.Color = 9;
            return new AnsiChar(_engine.State.StarChars[_engine.State.GameCycle & 0x3], tile.Color);
        }
    }
}