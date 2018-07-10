using Roton.Emulation.Data;

namespace Roton.Emulation.SuperZzt
{
    public class SuperZztLocker
    {
        private readonly IActors _actors;

        public SuperZztLocker(IActors actors)
        {
            _actors = actors;
        }
        
        public void Lock(int index)
        {
            _actors[index].P3 = 1;
        }

        public void Unlock(int index)
        {
            _actors[index].P3 = 0;
        }

        public bool IsLocked(int index)
        {
            return _actors[index].P3 != 0;
        }
    }
}