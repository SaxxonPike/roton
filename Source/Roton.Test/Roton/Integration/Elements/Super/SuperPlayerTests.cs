using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements.Super;

public class SuperPlayerTests : SuperContextTestFixture
{
    [Test]
    public void Player_ShouldBeAbleToInteractWithStone()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the water.
        SpawnTo(4, 3, Elements.StoneId);

        // Move the player into the stone.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(3, 3).Id.Should().Be(Elements.EmptyId,
            "player should have moved from original position");
        Stones.Should().Be(1,
            "player should have gained 1 stone");
        Message.Should().BeEquivalentTo(Alerts.StoneMessage.Text,
            "correct message should be displayed");
    }

}