using Roton.Emulation;

namespace Roton
{
    public interface IActor : ICodeSeekable, ICode
    {
        int Cycle { get; set; }
        int Follower { get; set; }
        int Leader { get; set; }
        int Length { get; set; }
        IXyPair Location { get; }
        int P1 { get; set; }
        int P2 { get; set; }
        int P3 { get; set; }
        int Pointer { get; set; }
        Tile UnderTile { get; }
        IXyPair Vector { get; }
        int X { get; set; }
        int Y { get; set; }
        void CopyFrom(IActor other);
        bool IsAttached { get; }
    }
}
