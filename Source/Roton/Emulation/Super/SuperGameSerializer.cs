using System;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super
{
    [Context(Context.Super)]
    public sealed class SuperGameSerializer : GameSerializer
    {
        public SuperGameSerializer(Lazy<IMemory> memory, Lazy<IHeap> heap)
            : base(memory, heap)
        {
        }

        public override int ActorCapacity => 129;
        public override int ActorDataCountOffset => 0x6AB3;
        public override int ActorDataLength => 0x19;
        public override int ActorDataOffset => 0x6AB5;
        public override int BoardDataLength => 0x1C;
        public override int BoardDataOffset => 0x7767;
        public override int BoardNameLength => 0x3D;
        public override int BoardNameOffset => 0x2BAE;
        public override int WorldDataCapacity => 0x400;
        public override int WorldDataOffset => 0x784C;
        public override int WorldDataSize => 0x0187;
    }
}