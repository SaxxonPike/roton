using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Roton.Integration.Elements;

public class BlinkWallTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void BlinkWall_ShouldEmitHorizontalRay_WhenTriggered()
    {
        // Blink rays fire toward a vector until the ray collides with a non-empty.

        // Spawn a blink wall.
        var wallIndex = SpawnTo(5, 5, ElementList.BlinkWallId);
        var wall = Actors[wallIndex];
        wall.Vector = Vector.East;
        wall.P1 = 0;
        wall.P2 = 1;
        wall.P3 = 1;
        wall.Cycle = 1;

        // Spawn a solid wall for the blink wall to fire into.
        PlotTo(8, 5, ElementList.SolidId);

        // Wait for the ray to be emitted.
        Step();

        // Assert.
        TileAt(6, 5).Id.Should().Be(ElementList.BlinkRayHId,
            "horizontal ray should be spawned 1 tile away");
        TileAt(7, 5).Id.Should().Be(ElementList.BlinkRayHId,
            "horizontal ray should be spawned 2 tiles away");
        TileAt(8, 5).Id.Should().Be(ElementList.SolidId,
            "walls should stop rays");
    }

    [Test]
    public void BlinkWall_ShouldRemoveHorizontalRay_OnNextRayCycle()
    {
        // If a ray exists, blink walls will remove the ray on their cycle.
        // The ray must be the same color as the blink wall.

        // Spawn a blink wall.
        var wallIndex = SpawnTo(5, 5, ElementList.BlinkWallId, 0x01);
        var wall = Actors[wallIndex];
        wall.Vector = Vector.East;
        wall.P1 = 0;
        wall.P2 = 1;
        wall.P3 = 1;
        wall.Cycle = 1;

        // Spawn the ray.
        PlotTo(6, 5, ElementList.BlinkRayHId, 0x01);
        PlotTo(7, 5, ElementList.BlinkRayHId, 0x01);

        // Wait for the ray to be removed.
        Step();

        // Assert.
        TileAt(6, 5).Id.Should().Be(ElementList.EmptyId,
            "ray 1 tile away should be cleared");
        TileAt(7, 5).Id.Should().Be(ElementList.EmptyId,
            "ray 2 tiles away should be cleared");
    }

    [Test]
    public void BlinkWall_ShouldEmitVerticalRay_WhenFacingSouth()
    {
        // Vertical rays work like horizontal rays, but the element ID for the ray is different.

        var wallIndex = SpawnTo(5, 5, ElementList.BlinkWallId, 0x0E);
        var wall = Actors[wallIndex];
        wall.Vector = Vector.South;
        wall.P1 = 0;
        wall.P2 = 1;
        wall.P3 = 1;
        wall.Cycle = 1;

        PlotTo(5, 8, ElementList.SolidId);

        Step();

        TileAt(5, 6).Id.Should().Be(ElementList.BlinkRayVId,
            "vertical ray should be spawned 1 tile away");
        TileAt(5, 7).Id.Should().Be(ElementList.BlinkRayVId,
            "vertical ray should be spawned 2 tiles away");
        TileAt(5, 8).Id.Should().Be(ElementList.SolidId,
            "walls should stop rays");
    }
    
    [Test]
    public void BlinkWall_ShouldRemoveVerticalRay_OnNextRayCycle()
    {
        // If a ray exists, blink walls will remove the ray on their cycle.
        // The ray must be the same color as the blink wall.

        // Spawn a blink wall.
        var wallIndex = SpawnTo(5, 5, ElementList.BlinkWallId, 0x01);
        var wall = Actors[wallIndex];
        wall.Vector = Vector.South;
        wall.P1 = 0;
        wall.P2 = 1;
        wall.P3 = 1;
        wall.Cycle = 1;

        // Spawn the ray.
        PlotTo(5, 6, ElementList.BlinkRayVId, 0x01);
        PlotTo(5, 7, ElementList.BlinkRayVId, 0x01);

        // Wait for the ray to be removed.
        Step();

        // Assert.
        TileAt(5, 6).Id.Should().Be(ElementList.EmptyId,
            "ray 1 tile away should be cleared");
        TileAt(5, 7).Id.Should().Be(ElementList.EmptyId,
            "ray 2 tiles away should be cleared");
    }

}