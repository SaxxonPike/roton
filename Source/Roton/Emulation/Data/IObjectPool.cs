namespace Roton.Emulation.Data;

public interface IObjectPool<T>
{
    T Rent();
    void Return(T obj);
}