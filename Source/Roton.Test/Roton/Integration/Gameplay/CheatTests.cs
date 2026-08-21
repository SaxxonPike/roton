using System.Linq;
using AwesomeAssertions;
using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Gameplay;

public class CheatTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void FlagCheat_ShouldSetFlag()
    {
        TypeCheat("+flag");
        Flags.AsEnumerable().Should().Contain(["FLAG"]);
    }
    
    [Test]
    public void MinusFlagCheat_ShouldClearFlag()
    {
        Flags[0] = "FLAG";
        TypeCheat("-flag");
        Flags.AsEnumerable().Should().NotContain(["FLAG"]);
    }
    
    [Test]
    public void GemsCheat_ShouldGiveGems()
    {
        TypeCheat("gems");
        Gems.Should().Be(5,
            "player should have gained gems");
    }

    [Test]
    public void AmmoCheat_ShouldGiveAmmo()
    {
        TypeCheat("ammo");
        Ammo.Should().Be(Facts.AmmoPerPickup,
            "player should have gained ammo");
    }
    
    [Test]
    public void HealthCheat_ShouldGiveHealth()
    {
        TypeCheat("health");
        Health.Should().Be(Facts.DefaultHealth + 50,
            "player should have gained health");
    }
    
    [Test]
    public void TimeCheat_ShouldDecreaseTimePassed()
    {
        TypeCheat("time");
        TimePassed.Should().Be(-30,
            "player should have received additional time");
    }
    
    [Test]
    public void KeysCheat_ShouldGiveAllKeys()
    {
        TypeCheat("keys");
        Keys[0].Should().BeTrue(
            "player should receive blue key");
        Keys[1].Should().BeTrue(
            "player should receive green key");
        Keys[2].Should().BeTrue(
            "player should receive cyan key");
        Keys[3].Should().BeTrue(
            "player should receive red key");
        Keys[4].Should().BeTrue(
            "player should receive purple key");
        Keys[5].Should().BeTrue(
            "player should receive yellow key");
        Keys[6].Should().BeTrue(
            "player should receive white key");
    }

    [Test]
    public void ZapCheat_ShouldClearAdjacentTilesToPlayer()
    {
        // Place the player.
        MovePlayerTo(10, 10);
        
        // Place walls around the player.
        PlotTo(9, 9, Elements.SolidId);
        PlotTo(9, 10, Elements.SolidId);
        PlotTo(9, 11, Elements.SolidId);
        PlotTo(10, 9, Elements.SolidId);
        PlotTo(10, 11, Elements.SolidId);
        PlotTo(11, 9, Elements.SolidId);
        PlotTo(11, 10, Elements.SolidId);
        PlotTo(11, 11, Elements.SolidId);
        
        // Execute the cheat.
        TypeCheat("zap");
        
        // Assert.
        TileAt(9, 10).Id.Should().Be(Elements.EmptyId,
            "tile to the west should be cleared");
        TileAt(11, 10).Id.Should().Be(Elements.EmptyId,
            "tile to the east should be cleared");
        TileAt(10, 9).Id.Should().Be(Elements.EmptyId,
            "tile to the north should be cleared");
        TileAt(10, 11).Id.Should().Be(Elements.EmptyId,
            "tile to the south should be cleared");
        TileAt(9, 9).Id.Should().Be(Elements.SolidId,
            "other tiles should remain unchanged");
    }
}