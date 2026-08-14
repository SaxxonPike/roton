using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Roton.Integration.Elements;

public class PassageTests : ElementTestFixture
{
    public PassageTests(Context context) : base(context)
    {
    }

    [Test]
    public void Passage_ShouldSendPlayerToCorrectBoard()
    {
        // Set up board 1.
        GoToBoard(1);
        var passage1 = Actors[SpawnTo(2, 2, ElementList.PassageId, 1)];
        passage1.P3 = 0;
        MovePlayerTo(5, 2);

        // Set up board 0.
        GoToBoard(0);
        var passage0 = Actors[SpawnTo(2, 2, ElementList.PassageId, 1)];
        passage0.P3 = 1;
        MovePlayerTo(3, 2);

        // Walk the player into the passage and out of it on the destination board.
        Type(AnsiKey.Left);
        Type(AnsiKey.Right);

        // Play out steps.
        StepAllKeys();

        // Assert.
        World.BoardIndex.Should().Be(1,
            "player should arrive at the correct board");
        TileAt(5, 2).Id.Should().Be(ElementList.EmptyId,
            "player should leave the original location on the target board");
        TileAt(3, 2).Id.Should().Be(ElementList.PlayerId,
            "player should arrive at the passage");
    }

    [Test]
    public void Passage_ShouldSendPlayerToSameBoard()
    {
        // Set up left passage.
        var passage0 = Actors[SpawnTo(2, 2, ElementList.PassageId, 1)];
        passage0.P3 = 0;

        // Set up right passage.
        var passage1 = Actors[SpawnTo(4, 2, ElementList.PassageId, 1)];
        passage1.P3 = 0;

        // Set up passage of a different color.
        var passage2 = Actors[SpawnTo(6, 2, ElementList.PassageId, 2)];
        passage2.P3 = 0;

        // Walk the player into the passage and out of it.
        MovePlayerTo(3, 2);
        Type(AnsiKey.Left);
        Type(AnsiKey.Right);

        // Play out steps.
        StepAllKeys();

        // Assert.
        World.BoardIndex.Should().Be(0,
            "player should remain on the same board");
        TileAt(3, 2).Id.Should().Be(ElementList.EmptyId,
            "player should leave the original location");
        TileAt(5, 2).Id.Should().Be(ElementList.PlayerId,
            "player should arrive at the rightmost matching passage");
    }
}