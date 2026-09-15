using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IBoardPacker
{
    public int ActorCapacity { get; }
    public int ActorDataLength { get; }
    public int ActorDataOffset { get; }
    public int ActorDataCountOffset { get; }
    public int BoardDataLength { get; }
    public int BoardDataOffset { get; }
    public int BoardNameLength { get; }
    public int BoardNameOffset { get; }

    byte[] Pack();
    void Unpack(byte[] data);
}