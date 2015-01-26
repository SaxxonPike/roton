using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.SuperZZT
{
    sealed internal class Sounds : SoundsCommon
    {
        public override byte[] Forest
        {
            get
            {
                return new byte[]
                {
                    0x45, 0x01,
                    0x40, 0x01,
                    0x47, 0x01,
                    0x50, 0x01,
                    0x46, 0x01,
                    0x41, 0x01,
                    0x48, 0x01,
                    0x50, 0x01
                };
            }
        }
    }
}
