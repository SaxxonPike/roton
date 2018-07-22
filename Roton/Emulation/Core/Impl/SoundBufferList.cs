using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Core.Impl
{
    public sealed class SoundBufferList : FixedList<int>, ISoundBufferList
    {
        public SoundBufferList(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public override int Count => Memory.Read8(Offset);

        private IMemory Memory { get; }

        private int Offset { get; }

        protected override int GetItem(int index)
        {
            return Memory.Read8(Offset + index + 1);
        }

        protected override void SetItem(int index, int value)
        {
            Memory.Write8(Offset + index + 1, value);
        }

        public void Enqueue(ISound sound)
        {
            var totalLength = sound.Length + Memory.Read8(Offset);
            if (totalLength >= 255)
                return;

            var sourceIndex = 0;
            var targetIndex = Memory.Read8(Offset) + 1 + Offset;
            var remaining = sound.Length;
            while (remaining-- > 0)
                Memory.Write8(targetIndex++, sound[sourceIndex++]);
            Memory.Write8(Offset, totalLength);
        }

        public ISoundNote Dequeue()
        {
            var result = new SoundNote
            {
                Note = Memory.Read8(Offset + 1),
                Duration = Memory.Read8(Offset + 2)
            };
            var remaining = Memory.Read8(Offset) - 2;
            var index = Offset + 1;
            while (remaining-- > 0)
                Memory.Write8(index, Memory.Read8(index + 2));
            return result;
        }

        public override void Clear()
        {
            Memory.Write8(Offset, 0);
        }
    }
}