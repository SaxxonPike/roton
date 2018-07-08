using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class DuplicatorBehavior : ElementBehavior
    {
        private readonly IEngine _engine;

        public override string KnownName => KnownNames.Duplicator;

        public DuplicatorBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            var actor = _engine.Actors[index];
            var source = actor.Location.Sum(actor.Vector);
            var target = actor.Location.Difference(actor.Vector);

            if (actor.P1 > 4)
            {
                if (_engine.Tiles[target].Id == _engine.Elements.PlayerId)
                {
                    _engine.Tiles.ElementAt(source).Interact(source, 0, _engine.State.KeyVector);
                }
                else
                {
                    if (_engine.Tiles[target].Id != _engine.Elements.EmptyId)
                    {
                        _engine.Push(target, actor.Vector.Opposite());
                    }

                    if (_engine.Tiles[target].Id == _engine.Elements.EmptyId)
                    {
                        var sourceIndex = _engine.Actors.ActorIndexAt(source);
                        if (sourceIndex > 0)
                        {
                            if (_engine.State.ActorCount < _engine.Actors.Capacity - 2)
                            {
                                var sourceTile = _engine.Tiles[source];
                                _engine.SpawnActor(target, sourceTile, _engine.Elements[sourceTile.Id].Cycle,
                                    _engine.Actors[sourceIndex]);
                                _engine.UpdateBoard(target);
                            }
                        }
                        else if (sourceIndex != 0)
                        {
                            _engine.Tiles[target].CopyFrom(_engine.Tiles[source]);
                            _engine.UpdateBoard(target);
                        }

                        _engine.PlaySound(3, _engine.Sounds.Duplicate);
                    }
                    else
                    {
                        _engine.PlaySound(3, _engine.Sounds.DuplicateFail);
                    }
                }

                actor.P1 = 0;
            }
            else
            {
                actor.P1++;
            }

            _engine.UpdateBoard(actor.Location);
            actor.Cycle = (9 - actor.P2) * 3;
        }

        public override AnsiChar Draw(IXyPair location)
        {
            switch (_engine.ActorAt(location).P1)
            {
                case 2:
                    return new AnsiChar(0xF9, _engine.Tiles[location].Color);
                case 3:
                    return new AnsiChar(0xF8, _engine.Tiles[location].Color);
                case 4:
                    return new AnsiChar(0x6F, _engine.Tiles[location].Color);
                case 5:
                    return new AnsiChar(0x4F, _engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0xFA, _engine.Tiles[location].Color);
            }
        }
    }
}