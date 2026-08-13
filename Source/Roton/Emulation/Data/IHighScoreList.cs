using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface IHighScoreList : IEnumerable<IHighScore>
{
    IHighScore this[int index] { get; }
    bool Add(string name, int score);
}