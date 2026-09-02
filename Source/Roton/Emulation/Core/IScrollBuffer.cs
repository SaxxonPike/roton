namespace Roton.Emulation.Core;

public interface IScrollBuffer
{
    void Capture();
    void Restore(int y);
}