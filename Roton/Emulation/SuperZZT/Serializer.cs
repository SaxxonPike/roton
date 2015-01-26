using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roton.Emulation.SuperZZT
{
    sealed internal class Serializer : SerializerBase
    {
        public Serializer(Memory memory)
            : base(memory)
        {
        }

        public override int ActorCapacity
        {
            get { return 129; }
        }

        public override int ActorDataCountOffset
        {
            get { return 0x6AB3; }
        }

        public override int ActorDataLength
        {
            get { return 0x19; }
        }

        public override int ActorDataOffset
        {
            get { return 0x6AB5; }
        }

        public override int BoardDataLength
        {
            get { return 0x1C; }
        }

        public override int BoardDataOffset
        {
            get { return 0x7767; }
        }

        public override int BoardNameLength
        {
            get { return 0x3D; }
        }

        public override int BoardNameOffset
        {
            get { return 0x2BAE; }
        }

        public override int WorldDataCapacity
        {
            get { return 0x400; }
        }

        public override int WorldDataOffset
        {
            get { return 0x784C; }
        }

        public override int WorldDataSize
        {
            get { return 0x0187; }
        }
    }
}
