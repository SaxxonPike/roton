using Roton.FileIo;

namespace Roton.Common.Resources
{
    public class CommonResourceZipFileSystem : ZipFileSystem, ICommonResourceArchive
    {
        public CommonResourceZipFileSystem(byte[] file) : base(file)
        {
        }

        public byte[] GetPalette()
        {
            return GetFile(GetCombinedPath("system", "palette.bin"));
        }

        public byte[] GetFont()
        {
            return GetFile(GetCombinedPath("system", "font.bin"));
        }

        public byte[] GetDrumAudio(int index)
        {
            return GetFile(GetCombinedPath("audio", $"{index}.bin"));
        }

        public byte[] GetPlayerStepAudio()
        {
            return GetFile(GetCombinedPath("audio", "player.bin"));
        }
    }
}