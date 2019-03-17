namespace Roton.Emulation.Core
{
    public interface IComposerResourceService
    {
        byte[] GetPaletteData();
        byte[] GetFontData();
    }
}