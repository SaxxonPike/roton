using System.Linq;
using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Infrastructure;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration;

public class GameSerializerTests(Context context) : AllContextIntegrationTestFixture(context)
{
    [Test]
    public void PackAndUnpack_ShouldProduceCorrectBoard()
    {
        // Set up board.
        Engine.ClearBoard();

        // Add some code.
        var allCode = Enumerable.Range(0, 50).Select(x =>
        {
            var actor = SpawnTo(x + 1, 10, ElementList.ObjectId);
            var code = Create<string>();
            SetActorCode(actor, code);
            return code;
        }).ToList();
            
        // Pack that board.
        Engine.PackBoard();
            
        // Clear the board to setup for verification.
        Engine.ClearBoard();
            
        // Unpack that board.
        Engine.UnpackBoard(0);
            
        // Assert.
        Actors.Count.Should().Be(allCode.Count + 1);
        var observed = Actors.Select(x => x.Code?.ToStringValue()).Where(c => c != null).ToList();
        observed.Should().BeEquivalentTo(allCode);
    }
}