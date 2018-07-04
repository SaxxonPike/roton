using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class SlimeBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IGrid _grid;
        private readonly IElements _elements;
        private readonly IEngine _engine;
        private readonly IState _state;
        private readonly ISounds _sounds;

        public override string KnownName => KnownNames.Slime;

        public SlimeBehavior(IActors actors, IGrid grid, IElements elements, IEngine engine, IState state, ISounds sounds)
        {
            _actors = actors;
            _grid = grid;
            _elements = elements;
            _engine = engine;
            _state = state;
            _sounds = sounds;
        }
        
        public override void Act(int index)
        {
            var actor = _actors[index];

            if (actor.P1 >= actor.P2)
            {
                var spawnCount = 0;
                var color = _grid[actor.Location].Color;
                var slimeElement = _elements[_elements.SlimeId];
                var slimeTrailTile = new Tile(_elements.BreakableId, color);
                var source = actor.Location.Clone();
                actor.P1 = 0;

                for (var i = 0; i < 4; i++)
                {
                    var target = source.Sum(_engine.GetCardinalVector(i));
                    if (_grid.ElementAt(target).IsFloor)
                    {
                        if (spawnCount == 0)
                        {
                            _engine.MoveActor(index, target);
                            _grid[source].CopyFrom(slimeTrailTile);
                            _engine.UpdateBoard(source);
                        }
                        else
                        {
                            _engine.SpawnActor(target, new Tile(_elements.SlimeId, color), slimeElement.Cycle, null);
                            _actors[_state.ActorCount].P2 = actor.P2;
                        }
                        spawnCount++;
                    }
                }

                if (spawnCount == 0)
                {
                    _engine.RemoveActor(index);
                    _grid[source].CopyFrom(slimeTrailTile);
                    _engine.UpdateBoard(source);
                }
            }
            else
            {
                actor.P1++;
            }
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var color = _grid[location].Color;
            var slimeIndex = _actors.ActorIndexAt(location);
            _engine.Harm(slimeIndex);
            _grid[location].SetTo(_elements.BreakableId, color);
            _engine.UpdateBoard(location);
            _engine.PlaySound(2, _sounds.SlimeDie);
        }
    }
}