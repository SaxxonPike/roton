using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class EnergizerBehavior : ElementBehavior
    {
        public override string KnownName => "Energizer";

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            engine.PlaySound(9, engine.SoundSet.Energizer);
            engine.RemoveItem(location);
            engine.World.EnergyCycles = 0x4B;
            engine.UpdateStatus();
            engine.UpdateBoard(location);
            if (engine.Alerts.EnergizerPickup)
            {
                engine.Alerts.EnergizerPickup = false;
                engine.SetMessage(0xC8, engine.Alerts.EnergizerMessage);
            }
            engine.BroadcastLabel(0, @"ALL:ENERGIZE", false);
        }
    }
}