namespace Roton.Emulation.Execution
{
    public interface IRandomState
    {
        int Seed { get; }
        int State { get; set; }
    }
}