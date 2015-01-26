using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roton.Emulation.ZZT
{
    sealed internal class Serializer : SerializerBase
    {
        public Serializer(Memory memory)
            : base(memory)
        {
        }

        public override int ActorCapacity
        {
            get { return 152; }
        }

        public override int ActorDataCountOffset
        {
            get { return 0x31CD; }
        }

        public override int ActorDataLength
        {
            get { return 0x21; }
        }

        public override int ActorDataOffset
        {
            get { return 0x31CF; }
        }

        public override int BoardDataLength
        {
            get { return 0x56; }
        }

        public override int BoardDataOffset
        {
            get { return 0x4567; }
        }

        public override int BoardNameLength
        {
            get { return 0x33; }
        }

        public override int BoardNameOffset
        {
            get { return 0x2486; }
        }

        public override int WorldDataCapacity
        {
            get { return 0x200; }
        }

        public override int WorldDataOffset
        {
            get { return 0x481E; }
        }

        public override int WorldDataSize
        {
            get { return 0x0108; }
        }
    }
}
