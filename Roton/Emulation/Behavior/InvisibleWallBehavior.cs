using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class InvisibleWallBehavior : ElementBehavior
    {
        public override string KnownName => "Invisible Wall";

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            engine.Tiles[location].Id = engine.Elements.NormalId;
            engine.UpdateBoard(location);
            engine.PlaySound(3, engine.SoundSet.Invisible);
            engine.SetMessage(0x64, engine.Alerts.InvisibleMessage);
        }
    }
}