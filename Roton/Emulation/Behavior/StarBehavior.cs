using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class StarBehavior : EnemyBehavior
    {
        public override string KnownName => "Star";

        public override void Act(int index)
        {
            var actor = _actorList[index];

            actor.P2 = (actor.P2 - 1) & 0xFF;
            if (actor.P2 > 0)
            {
                if ((actor.P2 & 1) == 0)
                {
                    actor.Vector.CopyFrom(engine.Seek(actor.Location));
                    var targetLocation = actor.Location.Sum(actor.Vector);
                    var targetElement = engine.ElementAt(targetLocation);

                    if (targetElement.Id == engine.Elements.PlayerId || targetElement.Id == engine.Elements.BreakableId)
                    {
                        engine.Attack(index, targetLocation);
                    }
                    else
                    {
                        if (!targetElement.IsFloor)
                        {
                            engine.Push(targetLocation, actor.Vector);
                        }
                        if (targetElement.IsFloor || targetElement.Id == engine.Elements.WaterId)
                        {
                            engine.MoveActor(index, targetLocation);
                        }
                    }
                }
                else
                {
                    engine.UpdateBoard(actor.Location);
                }
            }
            else
            {
                engine.RemoveActor(index);
            }
        }

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            var tile = engine.Tiles[location];
            tile.Color++;
            if (tile.Color > 15)
                tile.Color = 9;
            return new AnsiChar(engine.State.StarChars[engine.State.GameCycle & 0x3], tile.Color);
        }
    }
}