namespace Roton.Core
{
    public interface IActor : ICodeInstruction, ICode
    {
        IXyPair Location { get; }
        ITile UnderTile { get; }
        IXyPair Vector { get; }
        int Cycle { get; set; }
        int Follower { get; set; }
        int Leader { get; set; }
        int Length { get; set; }
        int P1 { get; set; }
        int P2 { get; set; }
        int P3 { get; set; }
        int Pointer { get; set; }
    }
}