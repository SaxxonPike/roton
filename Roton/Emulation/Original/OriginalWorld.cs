using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [ContextEngine(ContextEngine.Original)]
    public sealed class OriginalWorld : IWorld
    {
        public OriginalWorld(IMemory memory, IKeyList keyList, IFlags flags)
        {
            Memory = memory;
            Keys = keyList;
            Flags = flags;
        }

        private IMemory Memory { get; }

        public int Ammo
        {
            get => Memory.Read16(0x481E);
            set => Memory.Write16(0x481E, value);
        }

        public int BoardIndex
        {
            get => Memory.Read16(0x482B);
            set => Memory.Write16(0x482B, value);
        }

        public int EnergyCycles
        {
            get => Memory.Read16(0x4831);
            set => Memory.Write16(0x4831, value);
        }

        public int Gems
        {
            get => Memory.Read16(0x4820);
            set => Memory.Write16(0x4820, value);
        }

        public int Health
        {
            get => Memory.Read16(0x4829);
            set => Memory.Write16(0x4829, value);
        }

        public bool IsLocked
        {
            get => Memory.ReadBool(0x4922);
            set => Memory.WriteBool(0x4922, value);
        }

        public IFlags Flags { get; }
        
        public IKeyList Keys { get; }

        public string Name
        {
            get => Memory.ReadString(0x4837);
            set => Memory.WriteString(0x4837, value);
        }

        public int Score
        {
            get => Memory.Read16(0x4835);
            set => Memory.Write16(0x4835, value);
        }

        public int Stones
        {
            get => 0;
            set { }
        }

        public int TimePassed
        {
            get => Memory.Read16(0x491E);
            set => Memory.Write16(0x491E, value);
        }

        public int TorchCycles
        {
            get => Memory.Read16(0x482F);
            set => Memory.Write16(0x482F, value);
        }

        public int Torches
        {
            get => Memory.Read16(0x482D);
            set => Memory.Write16(0x482D, value);
        }

        public int WorldType => -1;

        public override string ToString() => Name ?? base.ToString();
    }
}