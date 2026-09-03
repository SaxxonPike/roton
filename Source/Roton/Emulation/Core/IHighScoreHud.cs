using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IHighScoreHud
{
    string? EnterHighScore(IHighScoreList highScoreList, int score);
    void ShowHighScores(IHighScoreList highScoreList);

}