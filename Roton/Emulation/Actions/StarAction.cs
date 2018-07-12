using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Original, 0x0F)]
    [ContextEngine(ContextEngine.Super, 0x48)]
    public sealed class StarAction : IAction
    {
        private readonly IEngine _engine;
        
        public StarAction(IEngine engine)
        {
            _engine = engine;
        }

        public void Act(int index)
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

                    if (targetElement.Id == _engine.ElementList.PlayerId || targetElement.Id == _engine.ElementList.BreakableId)
                    {
                        _engine.Attack(index, targetLocation);
                    }
                    else
                    {
                        if (!targetElement.IsFloor)
                        {
                            _engine.Push(targetLocation, actor.Vector);
                        }

                        if (targetElement.IsFloor || targetElement.Id == _engine.ElementList.WaterId)
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
    }
}