using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Data.Impl;

public class HighScoreList : IHighScoreList
{
    private readonly int _count;
    private readonly List<IHighScore> _list;

    internal HighScoreList(int count)
    {
        _count = count;
        _list = Enumerable
            .Range(0, count)
            .Select(_ => new HighScore {Name = string.Empty, Score = -1})
            .Cast<IHighScore>()
            .ToList();
    }
        
    public IEnumerator<IHighScore> GetEnumerator() => 
        _list.GetEnumerator();

    public IHighScore this[int index] => _list[index];

    public bool Add(string name, int score)
    {
        var i = 0;
        while (i < _count)
        {
            if (_list[i].Score <= score)
            {
                for (var j = _count - 1; j > i; j--)
                {
                    var a = _list[j];
                    var b = _list[j - 1];
                    a.Name = b.Name;
                    a.Score = b.Score;
                }

                _list[i].Name = name;
                _list[i].Score = unchecked((short) score);
                return true;
            }

            i++;
        }

        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}