using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core.Impl;

namespace Roton.Test.Roton.Integration.Elements;

public class InvisibleWallTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void InvisibleWall_ShouldBecomeNormalWall_WhenTouchedByPlayer()
    {
        // Place the player.
        MovePlayerTo(3, 3);
        
        // Place the invisible wall.
        PlotTo(4, 3, ElementList.InvisibleId);

        // Move the player into the wall.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(4, 3).Id.Should().Be(ElementList.NormalId,
            "invisible wall should convert to a normal wall when touched");
        TileAt(3, 3).Id.Should().Be(ElementList.PlayerId,
            "player should remain in place and not move through invisible wall");
    }

    [Test]
    public void InvisibleWall_ShouldShowAlert_WhenTouchedByPlayer()
    {
        // Place the player.
        MovePlayerTo(3, 3);
        
        // Place the invisible wall.
        PlotTo(4, 3, ElementList.InvisibleId);

        // Move the player into the wall.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Message.Should().BeEquivalentTo(Alerts.InvisibleMessage.Text,
            "invisible wall alert message should be shown");
    }
}
