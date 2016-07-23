namespace Roton.Core
{
    public interface IMemory
    {
        IHeap Heap { get; }
        int Length { get; }
        byte[] Dump();
        byte[] Read(int offset, int length);
        int Read8(int offset);
        void Reset();
        void Write(int offset, byte[] data);
        void Write8(int offset, int value);
    }
}