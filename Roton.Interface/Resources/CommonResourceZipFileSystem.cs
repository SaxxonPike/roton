using Roton.FileIo;

namespace Roton.Interface.Resources
{
    public class CommonResourceZipFileSystem : ZipFileSystem, ICommonResourceArchive
    {
        public CommonResourceZipFileSystem(byte[] file) : base(file)
        {
        }

        public byte[] GetFont()
        {
            return GetFile(GetCombinedPath("system", "font.bin"));
        }

        public byte[] GetPalette()
        {
            return GetFile(GetCombinedPath("system", "palette.bin"));
        }
    }
}