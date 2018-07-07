using System;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Core
{
    public class Messager : IMessager
    {
        private readonly IActors _actors;
        private readonly IHud _hud;
        private readonly Lazy<ISpawner> _spawner;
        private readonly IState _state;
        private readonly IElements _elements;
        private readonly Lazy<IMover> _mover;
        private readonly IAlerts _alerts;
        private readonly ISounder _sounder;
        private readonly ISounds _sounds;

        public Messager(IActors actors, IHud hud, Lazy<ISpawner> spawner, IState state, IElements elements,
            Lazy<IMover> mover, IAlerts alerts, ISounder sounder, ISounds sounds)
        {
            _actors = actors;
            _hud = hud;
            _spawner = spawner;
            _state = state;
            _elements = elements;
            _mover = mover;
            _alerts = alerts;
            _sounder = sounder;
            _sounds = sounds;
        }

        public void SetMessage(int duration, IMessage message)
        {
            var index = _actors.ActorIndexAt(new Location(0, 0));
            if (index >= 0)
            {
                _mover.Value.RemoveActor(index);
                _hud.UpdateBorder();
            }

            var topMessage = message.Text[0];
            var bottomMessage = message.Text.Length > 1 ? message.Text[1] : string.Empty;

            _spawner.Value.SpawnActor(new Location(0, 0), new Tile(_elements.MessengerId, 0), 1, _state.DefaultActor);
            _actors[_state.ActorCount].P2 = duration / (_state.GameWaitTime + 1);
            _state.Message = topMessage;
            _state.Message2 = bottomMessage;
        }

        public void RaiseError(string error)
        {
            SetMessage(0xC8, _alerts.ErrorMessage(error));
            _sounder.Play(5, _sounds.Error);
        }
    }
}