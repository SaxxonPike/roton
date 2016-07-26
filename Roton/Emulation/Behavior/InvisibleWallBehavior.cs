using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class InvisibleWallBehavior : ElementBehavior
    {
        public override string KnownName => "Invisible Wall";

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            engine.Tiles[location].Id = engine.Elements.NormalId;
            engine.UpdateBoard(location);
            engine.PlaySound(3, engine.SoundSet.Invisible);
            engine.SetMessage(0x64, engine.Alerts.InvisibleMessage);
        }
    }
}