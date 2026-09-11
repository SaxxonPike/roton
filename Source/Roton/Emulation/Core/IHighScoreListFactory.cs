using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IHighScoreListFactory
{
    IHighScoreList Load();
    void Save(IHighScoreList highScoreList);
}