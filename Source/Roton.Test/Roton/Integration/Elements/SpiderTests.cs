using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core.Impl;

namespace Roton.Test.Roton.Integration.Elements;

public class SpiderTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void Spider_ShouldMoveOnWebTowardsPlayer_WhenSeeking()
    {
        // Spiders always move - if they can't get where they are going,
        // they will reverse direction or go perpendicular.

        RequireElement(ElementList.SpiderId);

        // Place the player.
        MovePlayerTo(10, 5);

        // Place a web path.
        PlotTo(5, 5, ElementList.WebId);
        PlotTo(6, 5, ElementList.WebId);
        PlotTo(7, 5, ElementList.WebId);

        // Place the spider on it.
        var spiderIndex = SpawnTo(5, 5, ElementList.SpiderId);
        var spider = Actors[spiderIndex];
        
        // Can't set P1=10 normally, but this guarantees that the spider will move
        // toward the player.
        spider.P1 = 10;
        spider.Cycle = 1;

        // Wait for the spider to activate.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(ElementList.WebId,
            "spider should leave web tile behind when moving");
        TileAt(6, 5).Id.Should().Be(ElementList.SpiderId,
            "spider should advance along web towards player");
    }

    [Test]
    public void Spider_ShouldNotMoveOntoEmptyFloor()
    {
        // Spiders can only move on webs.

        RequireElement(ElementList.SpiderId);

        // Place the player.
        MovePlayerTo(10, 5);
        
        // Place the spider.
        var spiderIndex = SpawnTo(5, 5, ElementList.SpiderId);
        var spider = Actors[spiderIndex];
        spider.P1 = 10;
        spider.Cycle = 1;

        // Wait for the spider to activate.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(ElementList.SpiderId,
            "spider should not move onto empty floor");
    }

    [Test]
    public void Spider_ShouldAttackPlayer_WhenAdjacent()
    {
        RequireElement(ElementList.SpiderId);

        // Place the player.
        MovePlayerTo(6, 5);
        
        // Place the spider.
        var spiderIndex = SpawnTo(5, 5, ElementList.SpiderId);
        var spider = Actors[spiderIndex];
        spider.P1 = 10;
        spider.Cycle = 1;

        // Wait for the spider to attack.
        var initialHealth = Health;
        Step();

        // Assert.
        Health.Should().Be(initialHealth - Facts.HealthLostPerHit,
            "player should take damage when spider attacks");
        TileAt(5, 5).Id.Should().Be(ElementList.EmptyId,
            "spider should be removed after attacking player");
    }

    [Test]
    public void Spider_ShouldDamagePlayer_WhenPlayerTouchesSpider()
    {
        RequireElement(ElementList.SpiderId);

        // Place the player.
        MovePlayerTo(3, 3);
        
        // Place the spider.
        SpawnTo(4, 3, ElementList.SpiderId);

        // Move the player into the spider.
        var initialHealth = Health;
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Health.Should().Be(initialHealth - Facts.HealthLostPerHit,
            "player should take damage when walking into spider");
        TileAt(3, 3).Id.Should().Be(ElementList.EmptyId,
            "player should move from original location");
        TileAt(4, 3).Id.Should().Be(ElementList.PlayerId,
            "player should occupy spider tile after destroying it");
    }
}
