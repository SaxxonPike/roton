namespace Roton.Emulation.Data
{
    public interface IPackedBoard
    {
        byte[] Data { get; set; }
        string Name { get; set; }
    }
}