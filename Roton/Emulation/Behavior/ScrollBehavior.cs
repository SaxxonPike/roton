using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class ScrollBehavior : ElementBehavior
    {
        public override string KnownName => "Scroll";

        public override void Act(int index)
        {
            var actor = _actorList[index];
            var color = engine.Tiles[actor.Location].Color;

            color++;
            if (color > 0x0F)
            {
                color = 0x09;
            }
            engine.Tiles[actor.Location].Color = color;
            engine.UpdateBoard(actor.Location);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var scrollIndex = engine.ActorIndexAt(location);
            var actor = _actorList[scrollIndex];

            engine.PlaySound(2, engine.EncodeMusic(@"c-c+d-d+e-e+f-f+g-g"));
            engine.ExecuteCode(scrollIndex, actor, @"Scroll");
            engine.RemoveActor(scrollIndex);
        }
    }
}