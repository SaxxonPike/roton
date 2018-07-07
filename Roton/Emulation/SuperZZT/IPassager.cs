using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
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
        private readonly IMessager _messager;

        public Passager(IBoard board, IActors actors, IAlerts alerts, IWorld world, IHud hud, IMessager messager)
        {
            _board = board;
            _actors = actors;
            _alerts = alerts;
            _world = world;
            _hud = hud;
            _messager = messager;
        }
        
        public void EnterBoard()
        {
            _board.Entrance.CopyFrom(_actors.Player.Location);
            if (_board.IsDark && _alerts.Dark)
            {
                _messager.SetMessage(0xC8, _alerts.DarkMessage);
                _alerts.Dark = false;
            }

            _world.TimePassed = 0;
            _hud.UpdateStatus();
        }
    }
}