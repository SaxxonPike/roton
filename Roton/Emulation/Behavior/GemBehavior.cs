﻿using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.SuperZZT;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class GemBehavior : ElementBehavior
    {
        private readonly IConfig _config;
        private readonly IWorld _world;
        private readonly ISounds _sounds;
        private readonly IAlerts _alerts;
        private readonly ISounder _sounder;
        private readonly IHud _hud;
        private readonly IMessager _messager;
        private readonly IMisc _misc;

        public GemBehavior(IConfig config, IWorld world, ISounds sounds, IAlerts alerts,
            ISounder sounder, IHud hud, IMessager messager, IMisc misc)
        {
            _config = config;
            _world = world;
            _sounds = sounds;
            _alerts = alerts;
            _sounder = sounder;
            _hud = hud;
            _messager = messager;
            _misc = misc;
        }

        public override string KnownName => KnownNames.Gem;

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _world.Health += _config.HealthPerGem;
            _world.Gems += 1;
            _world.Score += _config.ScorePerGem;
            _misc.RemoveItem(location);
            _hud.UpdateStatus();
            _sounder.Play(2, _sounds.Gem);

            if (!_alerts.GemPickup)
                return;

            _messager.SetMessage(0xC8, _alerts.GemMessage);
            _alerts.GemPickup = false;
        }
    }
}