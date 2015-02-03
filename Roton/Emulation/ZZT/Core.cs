using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.ZZT
{
    sealed internal partial class Core
    {
        IFileSystem _datArchive;

        void InitializeDatArchive()
        {
            _datArchive = new DatArchive(Properties.Resources.zztdat);
        }

        internal override byte[] LoadFile(string filename)
        {
            if (_datArchive.GetFiles().Contains(filename.ToLowerInvariant()))
            {
                return _datArchive.ReadFile(filename);
            }
            else
            {
                return base.LoadFile(filename);
            }
        }
    }
}
