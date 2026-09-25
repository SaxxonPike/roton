namespace Roton.Infrastructure;

public interface IFactory
{
    T Create<T>(params object?[] args);
}