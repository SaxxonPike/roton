using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Core
{
    public interface IPassager
    {
        void EnterBoard();
    }

    public class Passager : IPassager
    {
        private readonly IBoard _board;
        private readonly IActors _actors;
        private readonly IAlerts _alerts;
        private readonly IWorld _world;
        private readonly IHud _hud;
        private readonly IMessenger _messenger;

        public Passager(IBoard board, IActors actors, IAlerts alerts, IWorld world, IHud hud, IMessenger messenger)
        {
            _board = board;
            _actors = actors;
            _alerts = alerts;
            _world = world;
            _hud = hud;
            _messenger = messenger;
        }
        
        public void EnterBoard()
        {
            _board.Entrance.CopyFrom(_actors.Player.Location);
            if (_board.IsDark && _alerts.Dark)
            {
                _messenger.SetMessage(0xC8, _alerts.DarkMessage);
                _alerts.Dark = false;
            }

            _world.TimePassed = 0;
            _hud.UpdateStatus();
        }
    }
}