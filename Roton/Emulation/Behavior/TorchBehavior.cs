using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class TorchBehavior : ElementBehavior
    {
        public override string KnownName => "Torch";

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            engine.World.Torches++;
            engine.RemoveItem(location);
            engine.UpdateStatus();
            if (engine.Alerts.TorchPickup)
            {
                engine.SetMessage(0xC8, engine.Alerts.TorchMessage);
                engine.Alerts.TorchPickup = false;
            }
            engine.PlaySound(3, engine.SoundSet.Torch);
        }
    }
}