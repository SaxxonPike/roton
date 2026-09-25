namespace Roton.Emulation.Core;

public interface IBoardPacker
{
    byte[] Pack();
    int Unpack(byte[] data);
}