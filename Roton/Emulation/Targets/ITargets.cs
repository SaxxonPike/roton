namespace Roton.Emulation.Targets
{
    public interface ITargets
    {
        ITarget Get(string name);
        ITarget GetDefault();
    }
}