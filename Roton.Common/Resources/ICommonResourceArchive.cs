namespace Roton.Common.Resources
{
    public interface ICommonResourceArchive
    {
        byte[] GetDrumAudio(int index);
        byte[] GetPlayerStepAudio();
        byte[] GetPalette();
        byte[] GetFont();
    }
}