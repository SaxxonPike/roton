namespace Roton.Emulation.ZZT
{
    internal sealed class Serializer : SerializerBase
    {
        public Serializer(Memory memory)
            : base(memory)
        {
        }

        public override int ActorCapacity => 152;

        public override int ActorDataCountOffset => 0x31CD;

        public override int ActorDataLength => 0x21;

        public override int ActorDataOffset => 0x31CF;

        public override int BoardDataLength => 0x56;

        public override int BoardDataOffset => 0x4567;

        public override int BoardNameLength => 0x33;

        public override int BoardNameOffset => 0x2486;

        public override int WorldDataCapacity => 0x200;

        public override int WorldDataOffset => 0x481E;

        public override int WorldDataSize => 0x0108;
    }
}