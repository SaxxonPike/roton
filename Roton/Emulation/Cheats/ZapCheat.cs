using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Cheats
{
    public class ZapCheat : ICheat
    {
        private readonly IActors _actors;
        private readonly IMover _mover;
        private readonly ICompass _compass;

        public ZapCheat(IActors actors, IMover mover, ICompass compass)
        {
            _actors = actors;
            _mover = mover;
            _compass = compass;
        }
        
        public string Name => "ZAP";
        
        public void Execute()
        {
            for (var i = 0; i < 4; i++)
            {
                _mover.Destroy(_actors.Player.Location.Sum(_compass.GetCardinalVector(i)));
            }
        }
    }
}