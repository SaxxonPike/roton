namespace Roton.Emulation.Data
{
    public interface ITile
    {
        int Color { get; set; }
        int Id { get; set; }
        ITile Clone();
    }
}