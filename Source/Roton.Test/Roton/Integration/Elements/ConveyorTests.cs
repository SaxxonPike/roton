using AwesomeAssertions;
using NUnit.Framework;

namespace Roton.Test.Roton.Integration.Elements;

public class ConveyorTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void ClockwiseConveyor_ShouldConveyPushableClockwise()
    {
        // Place the conveyor.
        var conveyorIndex = SpawnTo(5, 5, ElementList.ClockwiseId);
        Actors[conveyorIndex].Cycle = 1;

        // Place a boulder next to it.
        PlotTo(5, 4, ElementList.BoulderId);

        // Wait for the conveyor to convey.
        Step();

        // Assert.
        TileAt(5, 4).Id.Should().Be(ElementList.EmptyId,
            "boulder should have moved from its previous location");
        TileAt(6, 4).Id.Should().Be(ElementList.BoulderId,
            "boulder should have moved clockwise");
    }

    [Test]
    public void CounterClockwiseConveyor_ShouldConveyPushableCounterClockwise()
    {
        // Place the conveyor.
        var conveyorIndex = SpawnTo(5, 5, ElementList.CounterId);
        Actors[conveyorIndex].Cycle = 1;

        // Place a boulder next to it.
        PlotTo(5, 4, ElementList.BoulderId);

        // Wait for the conveyor to convey.
        Step();

        // Assert.
        TileAt(5, 4).Id.Should().Be(ElementList.EmptyId,
            "boulder should have been moved away from previous position");
        TileAt(4, 4).Id.Should().Be(ElementList.BoulderId,
            "boulder should have moved counter-clockwise");
    }
}