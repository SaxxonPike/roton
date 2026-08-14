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
        var passage1 = Actors[SpawnTo(1, 1, ElementList.PassageId, 1)];
        passage1.P3 = 0;
        MovePlayerTo(2, 1);

        // Set up board 0.
        GoToBoard(0);
        var passage0 = Actors[SpawnTo(1, 1, ElementList.PassageId, 1)];
        passage0.P3 = 1;
        MovePlayerTo(2, 1);

        // Walk the player into the passage and out of it on the destination board.
        Type(AnsiKey.Left);
        Type(AnsiKey.Right);

        // Play out steps.
        StepAllKeys();

        // Assert.
        World.BoardIndex.Should().Be(1);
    }
}