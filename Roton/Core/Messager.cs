using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Core
{
    public class Messager : IMessager
    {
        private readonly IActors _actors;
        private readonly IHud _hud;
        private readonly ISpawner _spawner;
        private readonly IState _state;
        private readonly IElements _elements;
        private readonly IMover _mover;
        private readonly IMessager _messager;
        private readonly IAlerts _alerts;
        private readonly ISounder _sounder;
        private readonly ISounds _sounds;

        public Messager(IActors actors, IHud hud, ISpawner spawner, IState state, IElements elements,
            IMover mover, IMessager messager, IAlerts alerts, ISounder sounder, ISounds sounds)
        {
            _actors = actors;
            _hud = hud;
            _spawner = spawner;
            _state = state;
            _elements = elements;
            _mover = mover;
            _messager = messager;
            _alerts = alerts;
            _sounder = sounder;
            _sounds = sounds;
        }

        public void SetMessage(int duration, IMessage message)
        {
            var index = _actors.ActorIndexAt(new Location(0, 0));
            if (index >= 0)
            {
                _mover.RemoveActor(index);
                _hud.UpdateBorder();
            }

            var topMessage = message.Text[0];
            var bottomMessage = message.Text.Length > 1 ? message.Text[1] : string.Empty;

            _spawner.SpawnActor(new Location(0, 0), new Tile(_elements.MessengerId, 0), 1, _state.DefaultActor);
            _actors[_state.ActorCount].P2 = duration / (_state.GameWaitTime + 1);
            _state.Message = topMessage;
            _state.Message2 = bottomMessage;
        }

        public void RaiseError(string error)
        {
            _messager.SetMessage(0xC8, _alerts.ErrorMessage(error));
            _sounder.Play(5, _sounds.Error);
        }
    }

    public interface IMessager
    {
        void RaiseError(string error);
        void SetMessage(int duration, IMessage message);
    }
}