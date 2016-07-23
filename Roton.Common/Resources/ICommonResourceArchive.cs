namespace Roton.Common.Resources
{
    public interface ICommonResourceArchive
    {
        byte[] GetDrumAudio(int index);
        byte[] GetFont();
        byte[] GetPalette();
        byte[] GetPlayerStepAudio();
    }
}