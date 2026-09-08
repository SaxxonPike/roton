using System.Collections.Generic;

namespace Roton.Editors;

public interface IActorEditor
{
    Vector Location { get; set; }
    Vector Vector { get; set; }
    int Cycle { get; set; }
    IDictionary<int, int> Parameters { get; }
    int Follower { get; set; }
    int Leader { get; set; }
    Tile UnderTile { get; set; }
    string Code { get; set; }
    int Instruction { get; set; }
    int BoundTo { get; set; }
}