namespace Roton.Emulation.Items
{
    public interface IItem
    {
        string Name { get; }
        int Value { get; set; }
    }
}