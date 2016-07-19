using Roton.Core;
using Roton.Emulation.Serialization;

namespace Roton.Emulation.ZZT
{
    internal sealed partial class Core
    {
        private IFileSystem _datArchive;

        private void InitializeDatArchive()
        {
            _datArchive = new DatArchive(Properties.Resources.zztdat);
        }

        internal override byte[] LoadFile(string filename)
        {
            if (_datArchive.GetFiles().Contains(filename.ToLowerInvariant()))
            {
                return _datArchive.ReadFile(filename);
            }
            return base.LoadFile(filename);
        }
    }
}