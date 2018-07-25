using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Original, 0x0C)]
    [ContextEngine(ContextEngine.Super, 0x0C)]
    public sealed class DuplicatorAction : IAction
    {
        private readonly IEngine _engine;

        public DuplicatorAction(IEngine engine)
        {
            _engine = engine;
        }

        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            var source = actor.Location.Sum(actor.Vector);
            var target = actor.Location.Difference(actor.Vector);

            if (actor.P1 > 4)
            {
                if (_engine.Tiles[target].Id == _engine.ElementList.PlayerId)
                {
                    _engine.InteractionList.Get(_engine.Tiles[source].Id)
                        .Interact(source, 0, _engine.State.KeyVector);
                }
                else
                {
                    if (_engine.Tiles[target].Id != _engine.ElementList.EmptyId)
                    {
                        _engine.Push(target, actor.Vector.Opposite());
                    }

                    if (_engine.Tiles[target].Id == _engine.ElementList.EmptyId)
                    {
                        var sourceIndex = _engine.Actors.ActorIndexAt(source);
                        if (sourceIndex > 0)
                        {
                            if (_engine.State.ActorCount < _engine.Actors.Capacity - 2)
                            {
                                var sourceTile = _engine.Tiles[source];
                                _engine.SpawnActor(target, sourceTile, _engine.ElementList[sourceTile.Id].Cycle,
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
    }
}