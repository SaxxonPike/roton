using Roton.Core;

namespace Roton.Emulation.SuperZZT
{
    public class ZztLocker : ILocker
    {
        private readonly IActors _actors;

        public ZztLocker(IActors actors)
        {
            _actors = actors;
        }
        
        public void Lock(int index)
        {
            _actors[index].P2 = 1;
        }

        public void Unlock(int index)
        {
            _actors[index].P2 = 0;
        }

        public bool IsLocked(int index)
        {
            return _actors[index].P2 != 0;
        }
    }
}