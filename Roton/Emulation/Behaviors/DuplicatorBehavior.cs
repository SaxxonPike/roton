using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class DuplicatorBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IElements _elements;
        private readonly ITiles _tiles;
        private readonly IState _state;
        private readonly ISounds _sounds;
        private readonly ISounder _sounder;
        private readonly IDrawer _drawer;
        private readonly ISpawner _spawner;
        private readonly IMover _mover;

        public override string KnownName => KnownNames.Duplicator;

        public DuplicatorBehavior(IActors actors, IElements elements, ITiles tiles, IState state,
            ISounds sounds, ISounder sounder, IDrawer drawer, ISpawner spawner, IMover mover)
        {
            _actors = actors;
            _elements = elements;
            _tiles = tiles;
            _state = state;
            _sounds = sounds;
            _sounder = sounder;
            _drawer = drawer;
            _spawner = spawner;
            _mover = mover;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];
            var source = actor.Location.Sum(actor.Vector);
            var target = actor.Location.Difference(actor.Vector);

            if (actor.P1 > 4)
            {
                if (_tiles[target].Id == _elements.PlayerId)
                {
                    _tiles.ElementAt(source).Interact(source, 0, _state.KeyVector);
                }
                else
                {
                    if (_tiles[target].Id != _elements.EmptyId)
                    {
                        _mover.Push(target, actor.Vector.Opposite());
                    }

                    if (_tiles[target].Id == _elements.EmptyId)
                    {
                        var sourceIndex = _actors.ActorIndexAt(source);
                        if (sourceIndex > 0)
                        {
                            if (_state.ActorCount < _actors.Capacity - 2)
                            {
                                var sourceTile = _tiles[source];
                                _spawner.SpawnActor(target, sourceTile, _elements[sourceTile.Id].Cycle,
                                    _actors[sourceIndex]);
                                _drawer.UpdateBoard(target);
                            }
                        }
                        else if (sourceIndex != 0)
                        {
                            _tiles[target].CopyFrom(_tiles[source]);
                            _drawer.UpdateBoard(target);
                        }

                        _sounder.Play(3, _sounds.Duplicate);
                    }
                    else
                    {
                        _sounder.Play(3, _sounds.DuplicateFail);
                    }
                }

                actor.P1 = 0;
            }
            else
            {
                actor.P1++;
            }

            _drawer.UpdateBoard(actor.Location);
            actor.Cycle = (9 - actor.P2) * 3;
        }

        public override AnsiChar Draw(IXyPair location)
        {
            switch (_actors.ActorAt(location).P1)
            {
                case 2:
                    return new AnsiChar(0xF9, _tiles[location].Color);
                case 3:
                    return new AnsiChar(0xF8, _tiles[location].Color);
                case 4:
                    return new AnsiChar(0x6F, _tiles[location].Color);
                case 5:
                    return new AnsiChar(0x4F, _tiles[location].Color);
                default:
                    return new AnsiChar(0xFA, _tiles[location].Color);
            }
        }
    }
}