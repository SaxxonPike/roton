using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.SuperZZT
{
    sealed internal class MemoryState : MemoryStateBase
    {
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

            _lineChars = new MemoryStringByteCollection(Memory, 0x22BA);
            _transporterHChars = new MemoryStringByteCollection(Memory, 0x1F64);
            _transporterVChars = new MemoryStringByteCollection(Memory, 0x1E64);
            _vector4 = new MemoryInt16Collection(Memory, 0x2250, 8);
            _vector8 = new MemoryInt16Collection(Memory, 0x2230, 16);
            _webChars = new MemoryStringByteCollection(Memory, 0x227C);
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
