namespace Roton.Emulation.Data.Impl
{
    public abstract class MemoryDrumBank : FixedList<IDrumSound>, IDrumBank
    {
        private readonly IMemory _memory;
        private readonly int _offset;

        protected MemoryDrumBank(IMemory memory, int offset)
        {
            _memory = memory;
            _offset = offset;

            GenerateSounds();
        }

        public override int Count => 10;

        protected override IDrumSound GetItem(int index)
        {
            return new MemoryDrumSound(_memory, _offset + index * 512, index);
        }

        private void GenerateSounds()
        {
            var sounds = new[]
            {
                new[] { 3200 },
                new[] { 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800, 1900, 2000, 2100, 2200, 2300, 2400 },
                new[] { 4800, 4800, 8000, 1600, 4800, 4800, 8000, 1600, 4800, 4800, 8000, 1600, 4800, 4800 },
                new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new[] { 500, 2556, 1929, 3776, 3386, 4517, 1385, 1103, 4895, 3396, 874, 1616, 5124, 606 },
                new[] { 1600, 1514, 1600, 821, 1600, 1715, 1600, 911, 1600, 1968, 1600, 1490, 1600, 1722 },
                new[] { 2200, 1760, 1760, 1320, 2640, 880, 2200, 1760, 1760, 1320, 2640, 880, 2200, 1760 },
                new[] { 688, 676, 664, 652, 640, 628, 616, 604, 592, 580, 568, 556, 544, 532 },
                new[] { 1207, 1224, 1163, 1127, 1159, 1236, 1269, 1314, 1127, 1224, 1320, 1332, 1257, 1327 },
                new[] { 378, 331, 316, 230, 224, 384, 480, 320, 358, 412, 376, 621, 554, 426 }
            };

            var offset = _offset;
            foreach (var sound in sounds)
            {
                var length = sound.Length;
                _memory.Write16(offset, length);
                for (var i = 0; i < length; i++)
                {
                    _memory.Write16(offset + 2 + (i << 1), sound[i]);
                }
                offset += 512;
            }
        }
    }
}
