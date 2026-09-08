using System;
using System.Collections.Generic;

namespace Roton.Editors;

public interface IBoardEditor
{
    int MaxShots { get; set; }
    bool IsDark { get; set; }
    IDictionary<Neighbor, int> Neighbors { get; }
    bool ReEnterWhenZapped { get; set; }
    string? Message { get; set; }
    Vector Enter { get; set; }
    TimeSpan TimeLimit { get; set; }

    IList<IActorEditor> Actors { get; }
}