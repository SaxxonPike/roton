using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.SuperZZT
{
    sealed internal class MemoryState : MemoryStateBase
    {
        Tile _borderTile;
        MemoryActor _defaultActor;
        MemoryTile _edgeTile;
        MemoryVector _keyVector;
        MemoryStringByteCollection _lineChars;
        MemoryStringByteCollection _transporterHChars;
        MemoryStringByteCollection _transporterVChars;
        MemoryInt16Collection _vector4;
        MemoryInt16Collection _vector8;
        MemoryStringByteCollection _webChars;

        public MemoryState(Memory memory)
            : base(memory)
        {
            memory.Write(0x0000, Properties.Resources.szztextra);

            _borderTile = new Tile(0, 0); //Super ZZT doesn't keep this in game memory; it's in code
            _defaultActor = new MemoryActor(Memory, 0x2262);
            _edgeTile = new MemoryTile(Memory, 0x2260);
            _keyVector = new MemoryVector(Memory, 0xCC6E);
            _lineChars = new MemoryStringByteCollection(Memory, 0x22BA);
            _transporterHChars = new MemoryStringByteCollection(Memory, 0x1F64);
            _transporterVChars = new MemoryStringByteCollection(Memory, 0x1E64);
            _vector4 = new MemoryInt16Collection(Memory, 0x2250, 8);
            _vector8 = new MemoryInt16Collection(Memory, 0x2230, 16);
            _webChars = new MemoryStringByteCollection(Memory, 0x227C);
        }

        public override Tile BorderTile
        {
            get
            {
                return _borderTile;
            }
            protected set
            {
            }
        }

        public override Actor DefaultActor
        {
            get
            {
                return _defaultActor;
            }
            protected set
            {
            }
        }

        public override Tile EdgeTile
        {
            get
            {
                return _edgeTile;
            }
            protected set
            {
            }
        }

        public override Vector KeyVector
        {
            get
            {
                return _keyVector;
            }
            set
            {
            }
        }

        public override IList<int> LineChars
        {
            get
            {
                return _lineChars;
            }
            protected set
            {
            }
        }

        public override IList<int> TransporterHChars
        {
            get
            {
                return _transporterHChars;
            }
            protected set
            {
            }
        }

        public override IList<int> TransporterVChars
        {
            get
            {
                return _transporterVChars;
            }
            protected set
            {
            }
        }

        public override IList<int> Vector4
        {
            get
            {
                return _vector4;
            }
            protected set
            {
            }
        }

        public override IList<int> Vector8
        {
            get
            {
                return _vector8;
            }
            protected set
            {
            }
        }

        public override IList<int> WebChars
        {
            get
            {
                return _webChars;
            }
            protected set
            {
            }
        }

    }
}
