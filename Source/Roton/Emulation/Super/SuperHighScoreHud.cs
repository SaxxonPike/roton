using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperHighScoreHud(
    IWorld world,
    ITextEntryHud textEntryHud,
    IScroll scroll,
    IScrollContent scrollContent)
    : IHighScoreHud
{
    private void AddHighScoreHeader()
    {
        scrollContent.AddLine("Score  Name");
        scrollContent.AddLine("-----  --------------------");
    }

    public string? EnterHighScore(IHighScoreList highScoreList, int score)
    {
        if (score <= 0 || !highScoreList.Any(hs => hs.Score <= score))
            return null;

        scrollContent.AddLines(
            string.Empty,
            " Enter your name:",
            string.Empty,
            string.Empty,
            string.Empty
        );

        string? name = null;
        scroll.ShowMessage($"New high score for {world.Name}",
            false,
            3,
            _ =>
            {
                name = textEntryHud.Show(12, 14, 15, 0x1E, 0x1F);
                return default;
            });
        return name;
    }

    public void ShowHighScores(IHighScoreList highScoreList)
    {
        AddHighScoreHeader();

        scrollContent.AddLines(
            highScoreList
                .Where(hs => !string.IsNullOrEmpty(hs.Name))
                .Select(hs => $"{hs.Score,5}  {hs.Name}"));

        scroll.ShowMessage($"High scores for {world.Name}", false, 0);
    }
}