namespace Roton.Infrastructure;

public interface ILazy<out T>
{
    T Value { get; }
}