using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements.Original;

public class OriginalPlayerTests : OriginalContextTestFixture
{
    [Test]
    public void Player_ShouldBeAbleToPickUpTorch()
    {
        if (Elements.TorchId < 0)
            Assert.Pass("Torch does not exist in this context");

        // Place the player.
        MovePlayerTo(3, 3);

        // Place the torch.
        PlotTo(4, 3, Elements.TorchId);

        // Move the player into the torch.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Torches.Should().Be(Facts.DefaultTorches + 1,
            "torch count should be correct");
        TileAt(4, 3).Id.Should().Be(Elements.PlayerId,
            "player should be in correct location after pickup");
        Message.Should().BeEquivalentTo(Alerts.TorchMessage.Text,
            "correct message should be displayed");
    }
    
    [Test]
    public void Player_ShouldBeAbleToInteractWithWater()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the water.
        PlotTo(4, 3, Elements.WaterId);

        // Move the player into the water.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(3, 3).Id.Should().Be(Elements.PlayerId,
            "player should be in correct location after interaction");
        Message.Should().BeEquivalentTo(Alerts.WaterMessage.Text,
            "correct message should be displayed");
    }
}