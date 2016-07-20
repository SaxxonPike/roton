using Roton.Extensions;

namespace Roton.Emulation.Mapping
{
    internal sealed class MemoryString
    {
        public static implicit operator string(MemoryString source)
        {
            return source.ToString();
        }

        public MemoryString(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public IMemory Memory { get; }

        public int Offset { get; }

        public override string ToString()
        {
            return Memory.ReadString(Offset);
        }
    }
}