using Roton.Emulation.Data;

namespace Roton.Emulation.Core
{
    public interface IMover
    {
        void Attack(int index, IXyPair location);
        void Destroy(IXyPair location);
        void Harm(int index);
        void MoveActor(int index, IXyPair target);
        void MoveActorOnRiver(int index);
        void RemoveActor(int index);
        void Push(IXyPair target, IXyPair vector);
        void Convey(IXyPair actorLocation, int direction);
        void PushThroughTransporter(IXyPair location, IXyPair vector);
    }
}