using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core;

namespace Roton.Test.Roton.Integration.Elements;

public class PassageTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void Passage_ShouldPauseWhenEntering()
    {
        // Place the player.
        MovePlayerTo(3, 2);

        // Set up the passage.
        var passage = Actors[SpawnTo(2, 2, Elements.PassageId, 1)];
        passage.P3 = 0;
        
        // Walk the player into the passage.
        Type(AnsiKey.Left);
        StepAllKeys();

        // Assert.
        GamePaused.Should().BeTrue(
            "game should pause when entering passage");
    }

    [Test]
    public void Passage_ShouldSendPlayerToCorrectBoard()
    {
        // Set up board 1.
        GoToBoard(1);
        var passage1 = Actors[SpawnTo(2, 2, Elements.PassageId, 1)];
        passage1.P3 = 0;
        MovePlayerTo(5, 2);

        // Set up board 0.
        GoToBoard(0);
        var passage0 = Actors[SpawnTo(2, 2, Elements.PassageId, 1)];
        passage0.P3 = 1;
        MovePlayerTo(3, 2);

        // Walk the player into the passage and out of it on the destination board.
        Type(AnsiKey.Left);
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        BoardIndex.Should().Be(1,
            "player should arrive at the correct board");
        TileAt(5, 2).Id.Should().Be(Elements.EmptyId,
            "player should leave the original location on the target board");
        TileAt(3, 2).Id.Should().Be(Elements.PlayerId,
            "player should arrive at the passage");
    }

    [Test]
    public void Passage_ShouldSendPlayerToSameBoard()
    {
        // Place the player.
        MovePlayerTo(3, 2);
        
        // Set up left passage.
        var passage0 = Actors[SpawnTo(2, 2, Elements.PassageId, 1)];
        passage0.P3 = 0;

        // Set up right passage.
        var passage1 = Actors[SpawnTo(4, 2, Elements.PassageId, 1)];
        passage1.P3 = 0;

        // Set up passage of a different color.
        var passage2 = Actors[SpawnTo(6, 2, Elements.PassageId, 2)];
        passage2.P3 = 0;

        // Walk the player into the passage and out of it.
        Type(AnsiKey.Left);
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        BoardIndex.Should().Be(0,
            "player should remain on the same board");
        TileAt(3, 2).Id.Should().Be(Elements.EmptyId,
            "player should leave the original location");
        TileAt(5, 2).Id.Should().Be(Elements.PlayerId,
            "player should arrive at the rightmost matching passage");
    }
}