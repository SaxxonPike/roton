using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.SuperZZT;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class EnergizerBehavior : ElementBehavior
    {
        private readonly ISounds _sounds;
        private readonly IAlerts _alerts;
        private readonly IWorld _world;
        private readonly IBroadcaster _broadcaster;
        private readonly ISounder _sounder;
        private readonly IHud _hud;
        private readonly IDrawer _drawer;
        private readonly IMessager _messager;
        private readonly IMisc _misc;

        public override string KnownName => KnownNames.Energizer;

        public EnergizerBehavior(ISounds sounds, IAlerts alerts, IWorld world, IBroadcaster broadcaster,
            ISounder sounder, IHud hud, IDrawer drawer, IMessager messager, IMisc misc)
        {
            _sounds = sounds;
            _alerts = alerts;
            _world = world;
            _broadcaster = broadcaster;
            _sounder = sounder;
            _hud = hud;
            _drawer = drawer;
            _messager = messager;
            _misc = misc;
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _sounder.Play(9, _sounds.Energizer);
            _misc.RemoveItem(location);
            _world.EnergyCycles = 0x4B;
            _hud.UpdateStatus();
            _drawer.UpdateBoard(location);
            if (_alerts.EnergizerPickup)
            {
                _alerts.EnergizerPickup = false;
                _messager.SetMessage(0xC8, _alerts.EnergizerMessage);
            }

            _broadcaster.BroadcastLabel(0, KnownLabels.Energize, false);
        }
    }
}