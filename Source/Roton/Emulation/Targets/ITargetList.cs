namespace Roton.Emulation.Targets
{
    public interface ITargetList
    {
        ITarget Get(string name);
    }
}