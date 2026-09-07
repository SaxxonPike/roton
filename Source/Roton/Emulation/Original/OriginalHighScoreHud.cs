using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalHighScoreHud(
    IWorld world,
    ILongTextEntryHud longTextEntryHud,
    IScroll scroll,
    IScrollContent scrollContent)
    : IHighScoreHud
{
    private void AddHighScoreHeader()
    {
        scrollContent.AddLine("Score  Name");
        scrollContent.AddLine("-----  ----------------------------------");
    }

    public string? EnterHighScore(IHighScoreList highScoreList, int score)
    {
        var index = -1;

        AddHighScoreHeader();

        var nameIndex = 2;

        foreach (var hs in highScoreList)
        {
            if (score > 0 && index < 0 && hs.Score <= score)
            {
                index = nameIndex;
                scrollContent.AddLine($"{score,5}  -- You! --");
            }

            if (string.IsNullOrEmpty(hs.Name))
                continue;

            scrollContent.AddLine($"{hs.Score,5}  {hs.Name}");
            nameIndex++;
        }

        if (index >= 0)
        {
            string? name = null;
            scroll.ShowMessage($"New high score for {world.Name}",
                false,
                2,
                _ =>
                {
                    name = longTextEntryHud.Show("Congratulations!  Enter your name:", 3, 18, 34, 0x4E, 0x4F);
                    return default;
                });
            return name;
        }

        return null;
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