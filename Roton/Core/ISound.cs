namespace Roton.Core
{
    public interface ISound
    {
        int this[int index] { get; }
        int Length { get; }
    }
}