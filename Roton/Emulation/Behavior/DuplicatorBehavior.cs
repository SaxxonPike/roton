using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class DuplicatorBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IElements _elements;
        private readonly IGrid _grid;
        private readonly IState _state;
        private readonly IEngine _engine;
        private readonly ISounds _sounds;

        public override string KnownName => KnownNames.Duplicator;

        public DuplicatorBehavior(IActors actors, IElements elements, IGrid grid, IState state, IEngine engine, ISounds sounds)
        {
            _actors = actors;
            _elements = elements;
            _grid = grid;
            _state = state;
            _engine = engine;
            _sounds = sounds;
        }
        
        public override void Act(int index)
        {
            var actor = _actors[index];
            var source = actor.Location.Sum(actor.Vector);
            var target = actor.Location.Difference(actor.Vector);

            if (actor.P1 > 4)
            {
                if (_grid.TileAt(target).Id == _elements.PlayerId)
                {
                    _grid.ElementAt(source).Interact(source, 0, _state.KeyVector);
                }
                else
                {
                    if (_grid.TileAt(target).Id != _elements.EmptyId)
                    {
                        _engine.Push(target, actor.Vector.Opposite());
                    }
                    if (_grid.TileAt(target).Id == _elements.EmptyId)
                    {
                        var sourceIndex = _actors.ActorIndexAt(source);
                        if (sourceIndex > 0)
                        {
                            if (_state.ActorCount < _actors.Capacity - 2)
                            {
                                var sourceTile = _grid.TileAt(source);
                                _engine.SpawnActor(target, sourceTile, _elements[sourceTile.Id].Cycle,
                                    _actors[sourceIndex]);
                                _engine.UpdateBoard(target);
                            }
                        }
                        else if (sourceIndex != 0)
                        {
                            _grid.TileAt(target).CopyFrom(_grid.TileAt(source));
                            _engine.UpdateBoard(target);
                        }
                        _engine.PlaySound(3, _sounds.Duplicate);
                    }
                    else
                    {
                        _engine.PlaySound(3, _sounds.DuplicateFail);
                    }
                }
                actor.P1 = 0;
            }
            else
            {
                actor.P1++;
            }

            _engine.UpdateBoard(actor.Location);
            actor.Cycle = (9 - actor.P2)*3;
        }

        public override AnsiChar Draw(IXyPair location)
        {
            switch (_actors.ActorAt(location).P1)
            {
                case 2:
                    return new AnsiChar(0xF9, _grid[location].Color);
                case 3:
                    return new AnsiChar(0xF8, _grid[location].Color);
                case 4:
                    return new AnsiChar(0x6F, _grid[location].Color);
                case 5:
                    return new AnsiChar(0x4F, _grid[location].Color);
                default:
                    return new AnsiChar(0xFA, _grid[location].Color);
            }
        }
    }
}