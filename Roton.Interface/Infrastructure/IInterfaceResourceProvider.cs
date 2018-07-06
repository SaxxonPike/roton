namespace Roton.Interface.Infrastructure
{
    public interface IInterfaceResourceProvider
    {
        byte[] GetPaletteData();
        byte[] GetFontData();        
    }
}