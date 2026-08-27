using AwesomeAssertions;
using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Oop;

public class PutTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    [TestCase("blue", 9)]
    [TestCase("green", 10)]
    [TestCase("cyan", 11)]
    [TestCase("red", 12)]
    [TestCase("purple", 13)]
    [TestCase("yellow", 14)]
    [TestCase("white", 15)]
    public void Put_ShouldPutColoredTile_NonDominant(string name, int expectedColor)
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            $"#put s {name} normal"
        );

        // Execute.
        Step();

        // Assert.
        TileAt(1, 2).Id.Should().Be(Elements.NormalId,
            "placed tile should be the correct element");
        TileAt(1, 2).Color.Should().Be(expectedColor,
            "placed tile should have the correct color");
    }

    [Test]
    [TestCase("blue", 0x1F)]
    [TestCase("green", 0x2F)]
    [TestCase("cyan", 0x3F)]
    [TestCase("red", 0x4F)]
    [TestCase("purple", 0x5F)]
    [TestCase("yellow", 0x6F)]
    [TestCase("white", 0x7F)]
    public void Put_ShouldPutColoredTile_ForegroundDominant(string name, int expectedColor)
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            $"#put s {name} door"
        );

        // Execute.
        Step();

        // Assert.
        TileAt(1, 2).Id.Should().Be(Elements.DoorId,
            "placed tile should be the correct element");
        TileAt(1, 2).Color.Should().Be(expectedColor,
            "placed tile should have the correct background color with dominant foreground color");
    }

    [Test]
    [TestCase("blue", 3)]
    [TestCase("green", 3)]
    [TestCase("cyan", 3)]
    [TestCase("red", 3)]
    [TestCase("purple", 3)]
    [TestCase("yellow", 3)]
    [TestCase("white", 3)]
    public void Put_ShouldPutColoredTile_Dominant(string name, int expectedColor)
    {
        // Place the test actor.
        var index = SpawnTo(1, 1, Elements.ObjectId);
        var actor = Actors[index];
        actor.Cycle = 1;
        SetActorCode(index,
            $"#put s {name} ammo"
        );

        // Execute.
        Step();

        // Assert.
        TileAt(1, 2).Id.Should().Be(Elements.AmmoId,
            "placed tile should be the correct element");
        TileAt(1, 2).Color.Should().Be(expectedColor,
            "placed tile should have the element's dominant color");
    }
}