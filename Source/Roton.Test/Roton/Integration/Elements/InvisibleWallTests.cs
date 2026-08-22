using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements;

public class InvisibleWallTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void InvisibleWall_ShouldBecomeNormalWall_WhenTouchedByPlayer()
    {
        // Place the player.
        MovePlayerTo(3, 3);
        
        // Place the invisible wall.
        PlotTo(4, 3, Elements.InvisibleId);

        // Move the player into the wall.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(4, 3).Id.Should().Be(Elements.NormalId,
            "invisible wall should convert to a normal wall when touched");
        TileAt(3, 3).Id.Should().Be(Elements.PlayerId,
            "player should remain in place and not move through invisible wall");
    }

    [Test]
    public void InvisibleWall_ShouldShowAlert_WhenTouchedByPlayer()
    {
        // Place the player.
        MovePlayerTo(3, 3);
        
        // Place the invisible wall.
        PlotTo(4, 3, Elements.InvisibleId);

        // Move the player into the wall.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Message.Should().BeEquivalentTo(Alerts.InvisibleMessage.Text,
            "invisible wall alert message should be shown");
    }
}
