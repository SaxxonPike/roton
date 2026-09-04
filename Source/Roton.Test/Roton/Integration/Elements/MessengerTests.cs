using AwesomeAssertions;
using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements;

public class MessengerTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void Messenger_DecrementsMessageCount_WhenMessageIsActive()
    {
        // The messenger uses P2 to track the duration of a message.

        // Place the messenger.
        var messengerId = SpawnTo(0, 0, Elements.MessengerId);
        var star = Actors[messengerId];
        star.Cycle = 1;
        star.P2 = 8;

        // Wait for the messenger to process.
        Step();

        // Assert.
        ((int)star.P2).Should().Be(7,
            "messenger P2 should have been decremented");
    }

    [Test]
    public void Messenger_CleansUp_WhenMessageDurationReachesZero()
    {
        // The messenger will remove itself when the duration counter reaches zero.

        // Place the messenger.
        var messengerId = SpawnTo(0, 0, Elements.MessengerId);
        var star = Actors[messengerId];
        star.Cycle = 1;
        star.P2 = 1;

        // Set a message.
        State.Message = "test message";

        // Wait for the messenger to process.
        Step();

        // Assert.
        State.Message.Should().BeEmpty(
            "message line 1 should have been cleared");
        State.Message2.Should().BeEmpty(
            "message line 2 should have been cleared");
        Actors.Count.Should().Be(1,
            "messenger should have been removed");
    }
}
