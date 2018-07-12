namespace Roton.Emulation.Data
{
    public interface ISound
    {
        int this[int index] { get; }
        int Length { get; }
    }
}