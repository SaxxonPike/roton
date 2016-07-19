namespace Roton.Emulation.Mapping
{
    internal sealed class MemoryString
    {
        public static implicit operator string(MemoryString source)
        {
            return source.ToString();
        }

        public MemoryString(Memory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public Memory Memory { get; }

        public int Offset { get; }

        public override string ToString()
        {
            return Memory.ReadString(Offset);
        }
    }
}