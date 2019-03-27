using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [Context(Context.Original)]
    public sealed class OriginalWorld : IWorld
    {
        private readonly IMemory _memory;
        
        public OriginalWorld(IMemory memory, IKeyList keyList, IFlags flags)
        {
            _memory = memory;
            Keys = keyList;
            Flags = flags;
        }

        public int Ammo
        {
            get => _memory.Read16(0x481E);
            set => _memory.Write16(0x481E, value);
        }

        public int BoardIndex
        {
            get => _memory.Read16(0x482B);
            set => _memory.Write16(0x482B, value);
        }

        public int EnergyCycles
        {
            get => _memory.Read16(0x4831);
            set => _memory.Write16(0x4831, value);
        }

        public int Gems
        {
            get => _memory.Read16(0x4820);
            set => _memory.Write16(0x4820, value);
        }

        public int Health
        {
            get => _memory.Read16(0x4829);
            set => _memory.Write16(0x4829, value);
        }

        public bool IsLocked
        {
            get => _memory.ReadBool(0x4922);
            set => _memory.WriteBool(0x4922, value);
        }

        public IFlags Flags { get; }
        
        public IKeyList Keys { get; }

        public string Name
        {
            get => _memory.ReadString(0x4837);
            set => _memory.WriteString(0x4837, value);
        }

        public int Score
        {
            get => _memory.Read16(0x4835);
            set => _memory.Write16(0x4835, value);
        }

        public int Stones
        {
            get => 0;
            set { }
        }

        public int TimePassed
        {
            get => _memory.Read16(0x491E);
            set => _memory.Write16(0x491E, value);
        }

        public int TorchCycles
        {
            get => _memory.Read16(0x482F);
            set => _memory.Write16(0x482F, value);
        }

        public int Torches
        {
            get => _memory.Read16(0x482D);
            set => _memory.Write16(0x482D, value);
        }

        public int WorldType => -1;

        public override string ToString() => Name ?? base.ToString();
    }
}