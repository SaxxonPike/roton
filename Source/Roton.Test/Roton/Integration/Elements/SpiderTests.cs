using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core;

namespace Roton.Test.Roton.Integration.Elements;

public class SpiderTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void Spider_ShouldMoveOnWebTowardsPlayer_WhenSeeking()
    {
        // Spiders always move - if they can't get where they are going,
        // they will reverse direction or go perpendicular.

        RequireElement(Elements.SpiderId);

        // Place the player.
        MovePlayerTo(10, 5);

        // Place a web path.
        PlotTo(5, 5, Elements.WebId);
        PlotTo(6, 5, Elements.WebId);
        PlotTo(7, 5, Elements.WebId);

        // Place the spider on it.
        var spiderIndex = SpawnTo(5, 5, Elements.SpiderId);
        var spider = Actors[spiderIndex];
        
        // Can't set P1=10 normally, but this guarantees that the spider will move
        // toward the player.
        spider.P1 = 10;
        spider.Cycle = 1;

        // Wait for the spider to activate.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.WebId,
            "spider should leave web tile behind when moving");
        TileAt(6, 5).Id.Should().Be(Elements.SpiderId,
            "spider should advance along web towards player");
    }

    [Test]
    public void Spider_ShouldNotMoveOntoEmptyFloor()
    {
        // Spiders can only move on webs.

        RequireElement(Elements.SpiderId);

        // Place the player.
        MovePlayerTo(10, 5);
        
        // Place the spider.
        var spiderIndex = SpawnTo(5, 5, Elements.SpiderId);
        var spider = Actors[spiderIndex];
        spider.P1 = 10;
        spider.Cycle = 1;

        // Wait for the spider to activate.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.SpiderId,
            "spider should not move onto empty floor");
    }

    [Test]
    public void Spider_ShouldAttackPlayer_WhenAdjacent()
    {
        RequireElement(Elements.SpiderId);

        // Place the player.
        MovePlayerTo(6, 5);
        
        // Place the spider.
        var spiderIndex = SpawnTo(5, 5, Elements.SpiderId);
        var spider = Actors[spiderIndex];
        spider.P1 = 10;
        spider.Cycle = 1;

        // Wait for the spider to attack.
        var initialHealth = Health;
        Step();

        // Assert.
        Health.Should().Be(initialHealth - Facts.HealthLostPerHit,
            "player should take damage when spider attacks");
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "spider should be removed after attacking player");
    }

    [Test]
    public void Spider_ShouldDamagePlayer_WhenPlayerTouchesSpider()
    {
        RequireElement(Elements.SpiderId);

        // Place the player.
        MovePlayerTo(3, 3);
        
        // Place the spider.
        SpawnTo(4, 3, Elements.SpiderId);

        // Move the player into the spider.
        var initialHealth = Health;
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Health.Should().Be(initialHealth - Facts.HealthLostPerHit,
            "player should take damage when walking into spider");
        TileAt(3, 3).Id.Should().Be(Elements.EmptyId,
            "player should move from original location");
        TileAt(4, 3).Id.Should().Be(Elements.PlayerId,
            "player should occupy spider tile after destroying it");
    }
}
