using Roton.Emulation.Mapping;
using Roton.Emulation.Serialization;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class Serializer : SerializerBase
    {
        public Serializer(IMemory memory)
            : base(memory)
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